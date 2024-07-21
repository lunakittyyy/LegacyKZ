using UnityEngine;

namespace CustomMaps
{
    public class Teleport : MonoBehaviour
    {
        public Transform point;

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<GorillaPlaySpace>())
            {
                CustomMaps.SetPosRot(point.position, point.rotation.eulerAngles.y);
            }
        }
    }
}
