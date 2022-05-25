using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LocationConfig
    {
        [Range(60f, 600f)] 
        public float LevelTimer = 300;
        [Range(40, 200)] 
        public int LevelSize = 80;
        public Location.EntityTemplates.WallTemplate WallPrefab;
        [Range(3, 36)] 
        public int WallsCount = 4;
        public int EnemiesCount = 10;
    }
}