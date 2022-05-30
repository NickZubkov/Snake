using UnityEngine;

namespace Modules.DragonIO.Data
{
    public static class GameConstants
    {
        public const int WALLS_COUNT = 60;
        public const float WALLS_ANGLE_OFFSET = 5f * Mathf.Deg2Rad;
        public const float WALL_SPAWN_RADIUS = 74.5f;
        public const float LEVEL_PREFAB_SIZE = 150f;
        public const float GROUND_DECOR_SPAWN_RADIUS = WALL_SPAWN_RADIUS * 0.9f;
        public const float OTHER_OBJECTS_SPAWN_RADIUS = WALL_SPAWN_RADIUS * 0.95f;
    }
}