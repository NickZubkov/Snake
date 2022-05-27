using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Components
{
    public struct LevelController
    {
        public Data.LevelsConfigs LevelsConfigs;
        public float BonusSpawnTimer;
        public List<Transform> GoodsPositions;
        public float WallSize;
        public float PlaceRadius;
        public Cinemachine.CinemachineTransposer CinemachineTransposer;
        public float DragonScalingFactor;
        public float LevelTimer;
        public int SpawnedEnemiesCount;
        public int PlayerPoints;
        public float ObjectsSpawnRadius;
    }
}