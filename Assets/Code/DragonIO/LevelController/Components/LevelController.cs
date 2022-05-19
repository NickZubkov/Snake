using System.Collections.Generic;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Components
{
    public struct LevelController
    {
        public float LevelTimer;
        public int MinFoodCount;
        public int MaxBonusCount;
        public float BonusMinSpawnTime;
        public float BonusMaxSpawnTime;
        public float BonusSpawnTimer;
        public List<Transform> GoodsPositions;
        public float PlaceRadius;
    }
}