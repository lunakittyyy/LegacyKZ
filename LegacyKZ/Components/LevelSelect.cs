using UnityEngine;
using UnityEngine.UI;

namespace CustomMaps
{
    public class LevelSelect : GorillaTriggerBox
    {
        private string levelName;

        private AssetBundle assetBundle;

        public void Init(string level, AssetBundle ab)
        {
            levelName = level;
            assetBundle = ab;
            GetComponentInChildren<Text>().text = levelName;
        }

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            StartCoroutine(global::CustomMaps.CustomMaps.instance.LoadMap(levelName, assetBundle));
        }
    }
}
