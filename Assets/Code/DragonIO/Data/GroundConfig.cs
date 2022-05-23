using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GroundConfig
    {
        public List<Obstacles.EntityTemplates.ObstacleTemplate> ObstaclePrefabs = new List<Obstacles.EntityTemplates.ObstacleTemplate>
        {
            null
        };
        public int ObstaclesCount;
        public List<Location.EntityTemplates.GroundTemplate> GroundPrefabs = new List<Location.EntityTemplates.GroundTemplate>
        {
            null
        };
        public int GroundCount;
    }
}