namespace Modules.DragonIO.Data
{
    [System.Serializable]
    public class EnemyConfig : DragonConfig
    {
        public EnemyAI EnemyAI;
        public float TimeToChangeDirection = 2;
        public float GoodsSerchRadius = 10f;
        public float SerchRadiusThreshold = 0.5f;
        public float ObstacleSerchingDistance = 5f;
    }
      public enum EnemyAI
        {
            Easy,
            Medium,
            Hard,
            Legend
        }
}