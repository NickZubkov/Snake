using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Modules/DragonIO/GameConfig")]
    
    public class GameConfig : SerializedScriptableObject
    {
        [TitleGroup("Levels")] 
        public LevelConfig LevelsConfigs;
        
        [Space]
        [FoldoutGroup("Other")]
        public float DragonScalingFactor = 0.02f;
        [FoldoutGroup("Other")]
        public Vector3 DefaultCameraOffset = new Vector3(0, 25f, 0);
    }

    [DictionaryDrawerSettings(KeyLabel = "Level",DisplayMode = DictionaryDisplayOptions.Foldout, ValueLabel = "LevelConfigs")]
    [System.Serializable]
    public class LevelConfig : UnitySerializedDictionary<LevelID, LevelsConfigs>
    {
        
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