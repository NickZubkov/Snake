using Sirenix.OdinInspector;
using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Modules/DragonIO/GameConfig")]
    
    public class GameConfig : ScriptableObject
    {
        [TitleGroup("Levels")] 
        public LevelsConfigs[] LevelsConfig;
        
    }
}