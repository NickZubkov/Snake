using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LevelsConfigs
    {
        public LocationConfig LocationConfig;
        public PlayerConfig PlayerConfig;
        public List<EnemyConfig> EnemiesConfigs;
        public GoodsConfig GoodsConfig;
        public ObstacleConfig ObstacleConfig;
    }
}