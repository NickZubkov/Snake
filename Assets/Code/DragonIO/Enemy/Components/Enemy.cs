
namespace Modules.DragonIO.Enemy.Components
{
    public struct Enemy
    {
        public Data.EnemyAI EnemyAI;
        public float TimeToChangeDirection;
        public float ChangeDirectionTimer;
        public float GoodsSerchRadius;
        public int MaxGoodsSerchingCount;
        public float ObstacleSerchingDistance;
        public int ObstacleLayerMask;
        public int GoodslayerMask;
        public bool IsAvoidingObstacle;
    }
}