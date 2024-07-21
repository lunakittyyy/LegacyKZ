using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using MelonLoader;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.XR;
using MelonLoader.Utils;
using System.Threading.Tasks;

[assembly: MelonInfo(typeof(CustomMaps.CustomMaps), "LegacyKZ", "0.1.3", "lunakitty")]

namespace CustomMaps
{
	public class CustomMaps : MelonMod
	{
		public static volatile CustomMaps instance;

		public static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();

		public static AssetBundle customMapsPrefabs;

		private bool displayGUI = false;

		private bool handTimer = false;

		private GameObject timer;

		private GameObject offlineVRRig;

		public static GorillaPlaySpace gps;

		private GorillaTagManager gtm;

		private Vector3 startingPos;

		private bool inCustomRoom = false;

		private GameObject map;

		private Vector3 levelSpawnPos = new Vector3(0f, 50f, 0f);

		private bool lastA;

		private bool lastB;

		private bool lastY;

		private bool lastMenu;

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        public override void OnLateInitializeMelon()
		{
			instance = this;

            LoggerInstance.Msg($"loading maps from: {MelonEnvironment.ModsDirectory}\\CustomMaps");
			DirectoryInfo directoryInfo = new DirectoryInfo($"{MelonEnvironment.ModsDirectory}\\CustomMaps");
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				LoggerInstance.Msg($"loading map: {fileInfo.FullName}");
				AssetBundle value = AssetBundle.LoadFromFile(fileInfo.FullName);
				assetBundles.Add(fileInfo.Name, value);
			}
			customMapsPrefabs = LoadAssetBundle("LegacyKZ.Resources.prefabs");
           
			gps = Object.FindObjectOfType<GorillaPlaySpace>();
            CheckpointManager.gps = gps;
            offlineVRRig = GameObject.Find("OfflineVRRig");
            startingPos = gps.transform.position;
            Quaternion rotation = Quaternion.Euler(0f, -90f, 0f);

            var fwdButton = customMapsPrefabs.LoadAsset("LevelSelectScrollForward") as GameObject;
            var bckButton = customMapsPrefabs.LoadAsset("LevelSelectScrollBackward") as GameObject;

            if (fwdButton == null)
                LoggerInstance.Error("forward button is null");
            if (bckButton == null)
                LoggerInstance.Error("back button is null");

            GameObject.Instantiate(fwdButton, new Vector3(-46f, 1.5f, -58.5f), rotation);
                                                                                         
            GameObject.Instantiate(bckButton, new Vector3(-46f, 1.5f, -58f), rotation);
            Physics.IgnoreLayerCollision(9, 12, ignore: false);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
			Object.Destroy(GameObject.Find("GorillaLobby/Geo/Boundaries/Ridge (71)"));
		}

		public override void OnUpdate()
		{
			if (Input.GetKeyDown(KeyCode.M))
			{
				displayGUI = !displayGUI;
			}
			if (Input.GetKeyDown(KeyCode.N))
			{
				Object.FindObjectOfType<GorillaTurning>().currentSpeed = 2f;
			}
			if (!gtm)
			{
				gtm = GorillaTagManager.instance;
			}
			else
			{
				if (!inCustomRoom)
				{
					return;
				}
				if (MapInfo.instance.gamemode == "gkz")
				{
					InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out var value);
					if (value && !lastA)
					{
						CheckpointManager.LoadCheckpoint();
					}
					lastA = value;
					if (!InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out var value2))
					{
						InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out value2);
					}
					if (value2 && !lastB)
					{
						CheckpointManager.SaveCheckpoint();
					}
					lastB = value2;
					if (!InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out var value3))
					{
						InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out value3);
					}
					if (value3 && !lastY)
					{
						CheckpointManager.DeleteCheckpoint();
					}
					lastY = value3;
				}
				InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.menuButton, out var value4);
				if (value4 && !lastMenu)
				{
					Disconnect();
				}
				lastMenu = value4;
				if (MapInfo.instance.isTagged)
				{
					gps.jumpMultiplier = MapInfo.instance.itJumpMultilier;
					gps.maxJumpSpeed = MapInfo.instance.itMaxJumpSpeed;
				}
				if (!MapInfo.instance.canTag)
				{
					gps.taggedTime = 0f;
					gps.tagCooldown = 0f;
				}
				gtm.fastJumpMultiplier = MapInfo.instance.itJumpMultilier;
				gtm.fastJumpLimit = MapInfo.instance.itMaxJumpSpeed;
				gtm.slowJumpMultiplier = MapInfo.instance.noobJumpMultiplier;
				gtm.slowJumpLimit = MapInfo.instance.noobMaxJumpSpeed;
			}
		}

		public override void OnGUI()
		{
			if (!displayGUI)
			{
				return;
			}
			if (GUI.Button(new Rect(1000f, 20f, 200f, 20f), "Disconnect"))
			{
				Disconnect();
			}
			GUI.Label(new Rect(1000f, 40f, 200f, 20f), "Custom Maps");
			int num = 60;
			foreach (KeyValuePair<string, AssetBundle> assetBundle in assetBundles)
			{
				if (GUI.Button(new Rect(1000f, num, 200f, 20f), assetBundle.Key))
				{
					MelonCoroutines.Start(LoadMap(assetBundle.Key, assetBundle.Value));
				}
				num += 20;
			}
			handTimer = GUI.Toggle(new Rect(1220f, 20f, 200f, 20f), handTimer, "Hand Timer");
		}

		public IEnumerator LoadMap(string levelName, AssetBundle levelAssetBundle)
		{
			if (PhotonNetwork.InRoom)
			{
				Disconnect();
				yield return new WaitForSeconds(1.5f);
			}
			offlineVRRig.SetActive(value: false);
			Object mapPrefab = levelAssetBundle.LoadAsset("Map");
			map = (GameObject)Object.Instantiate(mapPrefab);
			map.transform.position = levelSpawnPos;
			Transform spawn = map.transform.Find("Spawn");
			CheckpointManager.ResetCheckpoints();
			CheckpointManager.SaveCheckpoint(spawn.position, spawn.rotation.eulerAngles.y);
			CheckpointManager.LoadCheckpoint();
			MapInfo.instance = (MapInfo)map.GetComponent("CustomMaps.MapInfo");
			if (GorillaTagManager.instance == null)
			{
				gtm = new GameObject("GorillaTagManager").AddComponent<GorillaTagManager>();
				gtm.UpdateTagState();
			}
			Timer t = gps.GetComponent<Timer>();
			if (MapInfo.instance.gamemode == "gkz" && !t)
			{
				Object timerPrefab = customMapsPrefabs.LoadAsset("Timer");
				timer = (GameObject)Object.Instantiate(timerPrefab);
				if (handTimer)
				{
					timer.transform.parent = gps.leftHandTransform;
					timer.transform.localPosition = new Vector3(-0.08f, -0.2f, 0f);
					timer.transform.localRotation = Quaternion.Euler(180f, -90f, 0f);
				}
				else
				{
					timer.transform.parent = gps.headsetTransform;
					timer.transform.localPosition = new Vector3(0f, 0f, 0.5f);
					timer.transform.localRotation = Quaternion.identity;
				}
				gps.gameObject.AddComponent<Timer>();
			}
			if (MapInfo.instance.gamemode != "gkz" && (bool)t)
			{
				Object.Destroy(t);
				Object.Destroy(timer);
			}
			ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable { { "gameMode", "tagInfection" } };
			PhotonNetwork.JoinOrCreateRoom(levelName, new RoomOptions
			{
				IsVisible = false,
				IsOpen = true,
				MaxPlayers = 12,
				CustomRoomProperties = customRoomProperties
			}, TypedLobby.Default);
			inCustomRoom = true;
		}

		public void Disconnect()
		{
			GameObject.Find("QuitLobby").GetComponent<GorillaNetworkDisconnectTrigger>().OnBoxTriggered();
			inCustomRoom = false;
			gps.transform.position = startingPos;
			Object.Destroy(map);
			Object.Destroy(gps.GetComponent<Timer>());
			Object.Destroy(timer);
		}

		public static void SetPosRot(Vector3 pos, float rot)
		{
			gps.transform.rotation = Quaternion.Euler(0f, gps.transform.rotation.eulerAngles.y + rot - gps.headsetTransform.rotation.eulerAngles.y, 0f);
			gps.transform.position = gps.transform.position + pos - gps.headsetTransform.position;
			gps.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
		}

		public static string sha256_hash(string value)
		{
			StringBuilder stringBuilder = new StringBuilder();
			using (SHA256 sHA = SHA256.Create())
			{
				Encoding uTF = Encoding.UTF8;
				byte[] array = sHA.ComputeHash(uTF.GetBytes(value));
				byte[] array2 = array;
				foreach (byte b in array2)
				{
					stringBuilder.Append(b.ToString("x2"));
				}
			}
			return stringBuilder.ToString();
		}
	}
}
