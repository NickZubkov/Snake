
using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GroundConfig
    {
        public List<Obstacles.EntityTemplates.ObstacleTemplate> ObstaclePrefabs;
        public int ObstaclesCount;
        public List<Obstacles.EntityTemplates.GroundTemplate> GroundPrefabs;
        public int GroundCount;
    }
}