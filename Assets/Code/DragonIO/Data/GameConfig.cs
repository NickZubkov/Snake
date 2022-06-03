using System.Collections.Generic;
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
        [FoldoutGroup("Common")]
        public float DragonScalingFactor = 0.02f;
        [FoldoutGroup("Common")]
        public Vector3 DefaultCameraOffset = new Vector3(0, 25f, 0);
        [FoldoutGroup("Common")] 
        public float EnemyMinSpawnRadius = 15f;
        [FoldoutGroup("Common")] 
        public float ObstaclesMinSpawnRadius = 15f;
        [FoldoutGroup("Common")] 
        public float GoodsMinSpawnRadius = 15f;
        [FoldoutGroup("Common")]
        public int MaxGoodsSerchingCount = 15;
        [FoldoutGroup("Common")] 
        public float GoodsCollectingRadius = 2f;
        [FoldoutGroup("Common")] 
        public Location.EntityTemplates.DethVFXTemplate DethVFXPrefab;
        [FoldoutGroup("Common")][Range(1, 10)]
        public int BodyPartSpawnDecrease = 2;
        [FoldoutGroup("Common")] 
        public List<Sprite> CountrySprite;
        [FoldoutGroup("Common")]
        public List<string> Names;
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