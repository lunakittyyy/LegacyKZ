using System.Collections;
using UnityEngine;

namespace CustomMaps
{
	public class CoroutineManager : MonoBehaviour
	{
		private void StartCorutine(IEnumerator corutine)
		{
			StartCoroutine(corutine);
		}
	}
}

