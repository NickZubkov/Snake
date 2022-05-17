using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Modules/DragonIO/EnemyConfig")]
    public class EnemyConfig : DragonConfig
    {
        public EnemyAI EnemyAI;
        public int EnemyCount = 10;
        public float TimeToChangeDirection = 2;
    }

    public enum EnemyAI
    {
        Easy,
        Medium,
        Hard,
        Legend
    }
}