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
                
                foreach (var idx in _levelController)
                {
                    for (int i = 0; i < levelConfig.ObstacleConfig.ObstaclesCount; i++)
                    {
                        ref var controller = ref _levelController.Get1(idx);
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                        var obstacle = Object.Instantiate(levelConfig.ObstacleConfig.ObstaclePrefab, position, Quaternion.identity);
                        obstacle.Spawn(_world.NewEntity(), _world);
                    }
                    
                    _levelController.GetEntity(idx).Get<Components.ObstaclesSpawnedTag>();
                }
            }
        }
    }
}