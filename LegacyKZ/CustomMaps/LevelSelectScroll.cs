using System.Collections.Generic;
using CustomMaps;
using UnityEngine;

namespace LegacyKZ.CustomMaps
{
    public class LevelSelectScroll : GorillaTriggerBox
    {
        private static Vector3 levelSelectStart = new Vector3(-46.5f, 1.5f, -59.25f);

        private static Vector3 levelSelectOffset = new Vector3(-0.75f, 0f, 0f);

        public static Vector3 backwardButtonPos = new Vector3(-46f, 1.5f, -58f);

        public static Vector3 forwardButtonPos = new Vector3(-46f, 1.5f, -58.5f);

        public static int levelCount;

        public static int page = 0;

        private static int levelsPerPage = 5;

        private static Object levelSelectPreafab;

        private static List<GameObject> levelSelects = new List<GameObject>();

        public bool forwards;

        private float cooldownLength = 0.25f;

        private float cooldown;

        public void Start()
        {
            levelSelectPreafab = global::CustomMaps.CustomMaps.customMapsPrefabs.LoadAsset("LevelSelect");
            levelCount = global::CustomMaps.CustomMaps.assetBundles.Count;
            UpdateLevelSelects();
        }

        public void Update()
        {
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
            }
        }

        public static void UpdateLevelSelects()
        {
            foreach (GameObject levelSelect in levelSelects)
            {
                Object.Destroy(levelSelect);
            }
            levelSelects = new List<GameObject>();
            Vector3 position = levelSelectStart;
            int num = 0;
            foreach (KeyValuePair<string, AssetBundle> assetBundle in global::CustomMaps.CustomMaps.assetBundles)
            {
                if (num >= page * levelsPerPage && num < (page + 1) * levelsPerPage)
                {
                    GameObject gameObject = (GameObject)Object.Instantiate(levelSelectPreafab, position, Quaternion.identity);
                    gameObject.GetComponent<LevelSelect>().Init(assetBundle.Key, assetBundle.Value);
                    levelSelects.Add(gameObject);
                    position += levelSelectOffset;
                }
                num++;
            }
        }

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            if (cooldown <= 0f)
            {
                cooldown = cooldownLength;
                page += (forwards ? 1 : (-1));
                page = Mathf.Clamp(page, 0, (int)Mathf.Floor((levelCount - 1) / levelsPerPage));
                UpdateLevelSelects();
            }
        }
    }
}
