
namespace Modules.DragonIO.Enemy.Components
{
    public struct Enemy
    {
        public Data.EnemyAI EnemyAI;
        public float TimeToChangeDirection;
        public float ChangeDirectionTimer;
        public float GoodsSerchRadius;
        public float ObstacleSerchingDistance;
        public int ObstacleLayerMask;
        public bool IsAvoidingObstacle;
    }
}