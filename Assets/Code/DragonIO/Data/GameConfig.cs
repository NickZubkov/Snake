using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Modules/DragonIO/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [TitleGroup("Player")] 
        public Dragons.EntityTemplates.DragonHeadTemplate DragonHeadPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefab;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabFrontLegs;
        public Dragons.EntityTemplates.DragonBodyTemplate DragonBodyPrefabBackLegs;
        public DragonConfig[] DragonConfigs;
    }
}