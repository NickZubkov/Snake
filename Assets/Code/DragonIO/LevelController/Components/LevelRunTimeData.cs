using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Components
{
    public struct LevelRunTimeData
    {
        public float BonusSpawnTimer;
        public float WallSize;
        public float ObjectsMaxSpawnRadius;
        public Cinemachine.CinemachineTransposer CinemachineTransposer;
        public float DragonScalingFactor;
        public float LevelTimer;
        public int SpawnedEnemiesCount;
        public int PlayerPoints;
        public float EnemyMinSpawnRadiusSqr;
        public float ObstaclesMinSpawnRadiusSqr;
        public float GoodsMinSpawnRadiusSqr;
    }
}