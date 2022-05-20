using UnityEngine;

namespace Modules.DragonIO.Enemy.Components
{
    public struct Enemy
    {
        public Data.EnemyConfig EnemyConfig;
        public float TimeToChangeDirection;
        public float ChangeDirectionTimer;
        public Vector3 MoveDirection;
        public float SerchRadiusThreshold;
    }
}