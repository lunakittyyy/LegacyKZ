using UnityEngine;
using UnityEngine.UI;

namespace CustomMaps
{
    public class MapInfo : MonoBehaviour
    {
        public static MapInfo instance;

        public string mapName;

        public string gamemode = "inf";

        public string author;

        public Text leaderboard;

        public bool canTag = true;

        public bool isTagged;

        public float noobJumpMultiplier = 1.5f;

        public float noobMaxJumpSpeed = 6.5f;

        public float itJumpMultilier = 1.7f;

        public float itMaxJumpSpeed = 8.5f;
    }
}
