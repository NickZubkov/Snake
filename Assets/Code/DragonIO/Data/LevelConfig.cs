using UnityEngine;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LevelConfig
    {
        [Range(60f, 600f)] 
        public float LevelTimer = 300;
        [Range(40, 200)] 
        public int LevelSize = 80;
        public Level.EntityTemplates.WallTemplate WallPrefab;
        [Range(3, 36)] 
        public int WallsCount = 4;
    }
}