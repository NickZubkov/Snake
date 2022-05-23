using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Modules/DragonIO/GameConfig")]
    
    public class GameConfig : SerializedScriptableObject
    {
        [TitleGroup("Levels")] [DictionaryDrawerSettings(KeyLabel = "Level", ValueLabel = "LevelConfigs")]
        public Dictionary<LevelID, LevelsConfigs> LevelsConfigs = new Dictionary<LevelID, LevelsConfigs>
        {
            {
                LevelID.Level1, new LevelsConfigs
                {
                    LocationConfig = new LocationConfig(),
                    PlayerConfig = new PlayerConfig(),
                    EnemiesConfigs = new List<EnemyConfig>
                    {
                        new EnemyConfig()
                    },
                    GoodsConfig = new GoodsConfig(),
                    GroundConfig = new GroundConfig()
                }
            }
        };
    }
[System.Serializable]
    public enum LevelID
    {
        Level1 = 0,
        Level2,
        Level3,
        Level4,
        Level5,
        Level6,
        Level7,
        Level8,
        Level9,
        Level10
    }
}