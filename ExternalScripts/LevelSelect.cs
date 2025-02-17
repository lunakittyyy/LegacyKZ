﻿using UnityEngine;
using UnityEngine.UI;
using MelonLoader;

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
            GetComponentInChildren<TextMesh>().text = levelName;
        }

        public override void OnBoxTriggered()
        {
            base.OnBoxTriggered();
            StartCoroutine(CustomMaps.instance.LoadMap(levelName, assetBundle));
        }
    }
}