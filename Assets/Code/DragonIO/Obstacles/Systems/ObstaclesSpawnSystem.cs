using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Obstacles.Systems
{
    public class ObstaclesSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState, EventGroup.StateEnter> _enter;
        private EcsFilter<Components.Obstacle> _obstacles;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private EcsFilter<Components.ObstaclesSpawnedTag> _obstaclesSpawnedTag;

        private EcsWorld _world; 
        private Data.GameConfig _config;
        public void Run()
        {
            if (!_enter.IsEmpty() && _obstaclesSpawnedTag.IsEmpty())
            {
                var levelConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel);
                for (int i = 0; i < levelConfig.ObstacleConfig.ObstaclesCount; i++)
                {
                    var spawnPosition = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
                    var obstacle = Object.Instantiate(levelConfig.ObstacleConfig.ObstaclePrefab, spawnPosition, Quaternion.identity);
                    obstacle.Spawn(_world.NewEntity(), _world);
                }

                foreach (var idx in _levelController)
                {
                    _levelController.GetEntity(idx).Get<Components.ObstaclesSpawnedTag>();
                }
            }
        }
    }
}