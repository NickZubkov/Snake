using System.Collections.Generic;

namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class LevelsConfigs
    {
        public LocationConfig LocationConfig = new LocationConfig();
        public PlayerConfig PlayerConfig = new PlayerConfig();
        public List<EnemyConfig> EnemiesConfigs = new List<EnemyConfig>
        {
            new EnemyConfig()
        };
        public GoodsConfig GoodsConfig = new GoodsConfig();
        public GroundConfig GroundConfig = new GroundConfig();
    }
}