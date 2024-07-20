using UnityEngine;

namespace CustomMaps
{
    public class BHopController : MonoBehaviour
    {
        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("BHop"))
            {
                other.gameObject.GetComponent<BHopBlock>().Enter();
            }
        }

        public void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("BHop"))
            {
                other.gameObject.GetComponent<BHopBlock>().Exit();
            }
        }
    }
}
