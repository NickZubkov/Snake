
namespace Modules.DragonIO.Enemy.Components
{
    public struct Enemy
    {
        public Data.EnemyConfig EnemyConfig;
        public float TimeToChangeDirection;
        public float ChangeDirectionTimer;
        public float SerchRadiusThreshold;
        public int LayerMask;
        public bool IsAvoidingObstacle;
    }
}