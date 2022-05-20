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
        public void Run()
        {
            if (!_enter.IsEmpty() && _obstaclesSpawnedTag.IsEmpty())
            {
                foreach (var idx in _levelController)
                {
                    ref var controller = ref _levelController.Get1(idx);
                    for (int i = 0; i < controller.LevelsConfigs.ObstacleConfig.ObstaclesCount; i++)
                    {
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                        var obstacle = Object.Instantiate(controller.LevelsConfigs.ObstacleConfig.ObstaclePrefab, position, Quaternion.identity);
                        obstacle.Spawn(_world.NewEntity(), _world);
                    }
                    
                    _levelController.GetEntity(idx).Get<Components.ObstaclesSpawnedTag>();
                }
            }
        }
    }
}