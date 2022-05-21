using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Obstacles.Systems
{
    public class ObstaclesSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState, EventGroup.StateEnter> _enter;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private EcsFilter<Components.ObstaclesSpawnedTag> _obstaclesSpawnedTag;

        private EcsWorld _world;
        public void Run()
        {
            if (!_enter.IsEmpty() && _obstaclesSpawnedTag.IsEmpty())
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    for (int i = 0; i < controller.LevelsConfigs.GroundConfig.ObstaclesCount; i++)
                    {
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var idx = Random.Range(0, controller.LevelsConfigs.GroundConfig.ObstaclePrefabs.Count);
                        var prefab = controller.LevelsConfigs.GroundConfig.ObstaclePrefabs[idx];
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        var angle = new Vector3(0, Random.Range(0,180f), 0);
                        var obstacle = Object.Instantiate(prefab, position, Quaternion.Euler(angle));
                        obstacle.Spawn(_world.NewEntity(), _world);
                    }

                    for (int i = 0; i < controller.LevelsConfigs.GroundConfig.GroundCount; i++)
                    {
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var idx = Random.Range(0, controller.LevelsConfigs.GroundConfig.GroundPrefabs.Count);
                        var prefab = controller.LevelsConfigs.GroundConfig.GroundPrefabs[idx];
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        var angle = new Vector3(0, Random.Range(0,180f), 0);
                        var obstacle = Object.Instantiate(prefab, position, Quaternion.Euler(angle));
                        obstacle.Spawn(_world.NewEntity(), _world);
                    }
                    
                    _levelController.GetEntity(levelController).Get<Components.ObstaclesSpawnedTag>();
                }
            }
        }
    }
}