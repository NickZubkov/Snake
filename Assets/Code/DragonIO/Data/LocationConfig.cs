using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LocationConfig
    {
        [Range(10f, 600f)] 
        public float LevelTimer = 300;
        [Range(1, 10)] 
        public int LevelSize = 1;
        public Location.EntityTemplates.WallTemplate WallPrefab;
        public int EnemiesCount = 10;
    }
}