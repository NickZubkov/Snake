
namespace Modules.DragonIO.LevelController.Components
{
    public struct LevelRunTimeData
    {
        public float BonusSpawnTimer;
        public float WallSize;
        public float WallMaxSpawnRadius;
        public float GroundDecorMaxSpawnRadius;
        public float OtherObjectMaxSpawnRadius;
        public Cinemachine.CinemachineTransposer CinemachineTransposer;
        public float DragonScalingFactor;
        public float LevelTimer;
        public float WinFailTaimer;
        public int SpawnedEnemiesCount;
        public int PlayerPoints;
        public float EnemyMinSpawnRadiusSqr;
        public float ObstaclesMinSpawnRadiusSqr;
        public float GoodsMinSpawnRadiusSqr;
        public int GoodsLayerMask;
        public int MaxGoodsSerchingCount;
        public float GoodsCollectingRadius;
    }
}