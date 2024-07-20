using UnityEngine;

namespace CustomMaps
{
    public class BHopBlock : MonoBehaviour
    {
        private float decayTime = 0.25f;

        private float decayTimer;

        private float respawnTime = 1f;

        private float respawnTimer;

        private Renderer r;

        private Collider c;

        private void Start()
        {
            r = GetComponent<Renderer>();
            c = GetComponent<Collider>();
        }

        private void Update()
        {
            if (decayTimer > 0f)
            {
                decayTimer -= Time.deltaTime;
                if (decayTimer <= 0f)
                {
                    DisableBlock();
                    respawnTimer = respawnTime;
                }
            }
            if (respawnTimer > 0f)
            {
                respawnTimer -= Time.deltaTime;
                if (respawnTimer <= 0f)
                {
                    EnableBlock();
                }
            }
        }

        private void DisableBlock()
        {
            r.enabled = false;
            c.enabled = false;
        }

        private void EnableBlock()
        {
            r.enabled = true;
            c.enabled = true;
        }

        public void Enter()
        {
            if (decayTimer <= 0f)
            {
                decayTimer = decayTime;
            }
        }

        public void Exit()
        {
            decayTimer = -1f;
        }

        public void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Gorilla Player"))
            {
                Enter();
            }
        }

        public void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("Gorilla Player"))
            {
                Exit();
            }
        }
    }
}
