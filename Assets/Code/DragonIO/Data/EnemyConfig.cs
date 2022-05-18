using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Modules/DragonIO/EnemyConfig")]
    public class EnemyConfig : DragonConfig
    {
        public EnemyAI EnemyAI;
        public int EnemyCount = 10;
        public float TimeToChangeDirection = 2;
        public float GoodsSerchRadius = 5f;
        public float SerchRadiusThreshold = 0.5f;
    }

    public enum EnemyAI
    {
        Easy,
        Medium,
        Hard,
        Legend
    }
}