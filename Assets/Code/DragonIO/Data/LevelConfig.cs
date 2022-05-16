using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LevelConfig
    {
        public int LevelID = 0;
        [Range(60f, 600f)]
        public float LevelTimer = 300;

        public PlayerConfig PlayerConfig;
        public GoodsConfig GoodsConfig;
    }
}