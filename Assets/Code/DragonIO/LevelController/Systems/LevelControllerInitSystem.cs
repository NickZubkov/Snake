using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        private EcsFilter<Components.LevelController, Components.LevelControllerSpawnedSignal> _signal;
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            if (_controller.IsEmpty())
            {
                var controller = Object.Instantiate(_config.LevelControllerPrefab, Vector3.zero, Quaternion.identity);
                controller.Spawn(_world.NewEntity(), _world);
            }

            foreach (var idx in _signal)
            {
                ref var controller = ref _signal.Get1(idx);
                controller.LevelTimer = _config.LevelsConfig[0].LevelTimer;
                controller.MinFoodCount = _config.LevelsConfig[0].GoodsConfig.MinFoodCount;
                controller.MaxBonusCount = _config.LevelsConfig[0].GoodsConfig.MaxBonusCount;
                controller.BonusMinSpawnTime = _config.LevelsConfig[0].GoodsConfig.BonusSpawnTimeRange.Min;
                controller.BonusMaxSpawnTime = _config.LevelsConfig[0].GoodsConfig.BonusSpawnTimeRange.Max;
                controller.BonusSpawnTimer = Random.Range(controller.BonusMinSpawnTime, controller.BonusMaxSpawnTime);
            }
        }
    }
}