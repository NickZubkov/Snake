using UnityEngine;

namespace Modules.DragonIO.Enemy.Components
{
    public struct Enemy
    {
        public Data.EnemyConfig Config;
        public float ChangeDirectionTimeThreshold;
        public float ChangeDirectionTimer;
        public Vector3 MoveDirection;
        public float SerchRadiusThreshold;
    }
}