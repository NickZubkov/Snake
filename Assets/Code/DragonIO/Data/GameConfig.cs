using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Modules/DragonIO/GameConfig")]
    
    public class GameConfig : SerializedScriptableObject
    {
        [TitleGroup("Levels")] 
        [DictionaryDrawerSettings(KeyLabel = "Level", ValueLabel = "LevelConfigs")]
        public Dictionary<LevelID, LevelsConfigs> LevelsConfigs;
    }

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