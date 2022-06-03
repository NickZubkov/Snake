using System.Collections.Generic;

namespace Modules.DragonIO.LevelController.Components
{
    public struct CurrentLevelConfigs
    {
        public Data.LocationConfig LocationConfig;
        public Data.GroundConfig GroundConfig;
        public Data.PlayerConfig PlayerConfig;
        public List<Data.EnemyConfig> EnemiesConfigs;
        public Data.GoodsConfig GoodsConfig;
    }
}