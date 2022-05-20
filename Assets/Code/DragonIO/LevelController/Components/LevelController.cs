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
    }
}