using CustomMaps;
using UnityEngine;
using UnityEngine.UI;

namespace Timer
{
    public class Timer : MonoBehaviour
    {

        public float time = 5.45f;

        public bool isTiming;

        public Text text;

        private CustomMaps.Button[] buttons;

        public void StartTimer()
        {
            Reset();
            time = 0f;
            isTiming = true;
        }

        public void EndTimer()
        {
            if (isTiming)
            {
                Reset();
                CustomMaps.CustomMaps.gps.myVRRig.PlayTagSound(2);
            }
            isTiming = false;
        }

        public void ResetTimer()
        {
            Reset();
            time = 0f;
            isTiming = false;
        }

        public void Reset()
        {
            CheckpointManager.RemoveCheckpoints();
            CustomMaps.Button[] array = buttons;
            foreach (CustomMaps.Button button in array)
            {
                button.Reset();
            }
        }

        private void Awake()
        {
            text = GetComponentInChildren<Text>();
            buttons = UnityEngine.Object.FindObjectsOfType<CustomMaps.Button>();
        }

        private void Update()
        {
            if (isTiming)
            {
                time += Time.deltaTime;
            }
            string text = $"{(int)time / 60}:{time % 60f:00.000}";
            this.text.text = text;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name == "Start")
            {
                ResetTimer();
            }
            else if (other.name == "End")
            {
                EndTimer();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.name == "Start" && !isTiming)
            {
                StartTimer();
            }
        }
    }
}
