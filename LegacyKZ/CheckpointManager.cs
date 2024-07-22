using System.Collections.Generic;
using UnityEngine;

namespace CustomMaps
{
	public static class CheckpointManager
	{
		private static Stack<Vector3> checkpointsPos = new Stack<Vector3>();

		private static Stack<float> checkpointsRot = new Stack<float>();

		public static GorillaPlaySpace gps;

		public static void LoadCheckpoint()
		{
			CustomMaps.SetPosRot(checkpointsPos.Peek(), checkpointsRot.Peek());
		}

		public static void SaveCheckpoint()
		{
			if (Physics.Raycast(gps.headsetTransform.position, Vector3.down, 1f, 512))
			{
				checkpointsPos.Push(gps.headsetTransform.position);
				checkpointsRot.Push(gps.headsetTransform.rotation.eulerAngles.y);
				//gps.myVRRig.PlayTagSound(1);
			}
			else
			{
				//gps.myVRRig.PlayTagSound(0);
			}
		}

		public static void SaveCheckpoint(Vector3 pos, float rot)
		{
			checkpointsPos.Push(pos);
			checkpointsRot.Push(rot);
		}

		public static void DeleteCheckpoint()
		{
			if (checkpointsPos.Count > 1)
			{
				checkpointsPos.Pop();
				checkpointsRot.Pop();
			}
		}

		public static void RemoveCheckpoints()
		{
			while (checkpointsPos.Count > 1)
			{
				checkpointsPos.Pop();
				checkpointsRot.Pop();
			}
		}

		public static void ResetCheckpoints()
		{
			checkpointsPos = new Stack<Vector3>();
			checkpointsRot = new Stack<float>();
		}
	}
}