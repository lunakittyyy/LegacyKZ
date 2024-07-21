using UnityEngine;

namespace CustomMaps
{
    public class Button : GorillaTriggerBox
    {
        public GameObject[] toToggle;

        public bool setSelfColor = true;

        public bool setOtherColors = true;

        public Color color;

        private float cooldownLength = 0.5f;

        private float cooldown;

        private bool inDefaultState = true;

        public void Awake()
        {
            if (setSelfColor)
            {
                base.gameObject.GetComponent<Renderer>().material.color = this.color;
            }
            if (!setOtherColors)
            {
                return;
            }
            GameObject[] array = toToggle;
            foreach (GameObject gameObject in array)
            {
                Color color = gameObject.GetComponent<Renderer>().material.color;
                Color color2 = this.color;
                if (color != Color.white)
                {
                    color2 = (this.color + color) / 2f;
                }
                gameObject.GetComponent<Renderer>().material.color = color2;
            }
        }

        public void Update()
        {
            if (cooldown > 0f)
            {
                cooldown -= Time.deltaTime;
            }
        }

        public void Reset()
        {
            if (!inDefaultState)
            {
                inDefaultState = true;
                GameObject[] array = toToggle;
                foreach (GameObject gameObject in array)
                {
                    gameObject.SetActive(!gameObject.activeSelf);
                }
            }
        }

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            if (cooldown <= 0f)
            {
                cooldown = cooldownLength;
                inDefaultState = !inDefaultState;
                GameObject[] array = toToggle;
                foreach (GameObject gameObject in array)
                {
                    gameObject.SetActive(!gameObject.activeSelf);
                }
            }
        }
    }
}
