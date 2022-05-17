using UnityEngine;

namespace Modules.DragonIO.Data
{
    [CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Modules/DragonIO/ObstacleConfig")]
    public class ObstacleConfig : ScriptableObject
    {
        public Obstacles.EntityTemplates.ObstacleTemplate ObstaclePrefab;
        public int ObstaclesCount;
    }
}