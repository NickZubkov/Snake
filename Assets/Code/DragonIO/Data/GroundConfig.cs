using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class GroundConfig
    {
        public int ObstaclesCount;
        public List<Location.EntityTemplates.ObstacleTemplate> ObstaclePrefabs = new List<Location.EntityTemplates.ObstacleTemplate>
        {
            new Location.EntityTemplates.ObstacleTemplate()
        };
        
        public int GroundDecorCount;
        public List<Location.EntityTemplates.GroundDecorTemplate> GroundDecorPrefabs = new List<Location.EntityTemplates.GroundDecorTemplate>
        {
            new Location.EntityTemplates.GroundDecorTemplate()
        };
        
    }
}