using CustomMaps;
using UnityEngine;

namespace Teleport
{
    public class Teleport : MonoBehaviour
    {
        public Transform point;

        private void OnTriggerEnter(Collider other)
        {
            if ((bool)other.GetComponentInParent<GorillaPlaySpace>())
            {
                CustomMaps.CustomMaps.SetPosRot(point.position, point.rotation.eulerAngles.y);
            }
        }
    }
}
