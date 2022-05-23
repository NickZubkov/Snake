using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GroundConfig
    {
        public List<Obstacles.EntityTemplates.ObstacleTemplate> ObstaclePrefabs = new List<Obstacles.EntityTemplates.ObstacleTemplate>
        {
            new Obstacles.EntityTemplates.ObstacleTemplate()
        };
        public int ObstaclesCount;
        public List<Location.EntityTemplates.GroundTemplate> GroundPrefabs = new List<Location.EntityTemplates.GroundTemplate>
        {
            new Location.EntityTemplates.GroundTemplate()
        };
        public int GroundCount;
    }
}