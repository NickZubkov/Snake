using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        private EcsFilter<Enemy.Components.EnemiesSpawnedTag> _enemiesSpawnedTag;
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            if (_controller.IsEmpty())
            {
                var entity = _world.NewEntity();
                ref var controller = ref entity.Get<Components.LevelController>();
                entity.Get<LevelSpawner.LevelEntityTag>();
                var levelConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel);
                controller.LevelTimer = levelConfig.LevelConfig.LevelTimer;
                controller.MinFoodCount = levelConfig.GoodsConfig.MinFoodCount;
                controller.MaxBonusCount = levelConfig.GoodsConfig.MaxBonusCount;
                controller.BonusMinSpawnTime = levelConfig.GoodsConfig.BonusSpawnTimeRange.x;
                controller.BonusMaxSpawnTime = levelConfig.GoodsConfig.BonusSpawnTimeRange.y;
                controller.BonusSpawnTimer = Random.Range(controller.BonusMinSpawnTime, controller.BonusMaxSpawnTime);
                controller.GoodsPositions = new List<Transform>();
                var wallSize = levelConfig.LevelConfig.LevelSize * Mathf.Sin(180 * Mathf.Deg2Rad / levelConfig.LevelConfig.WallsCount);
                controller.PlaceRadius = wallSize / (2 * Mathf.Tan(180 * Mathf.Deg2Rad / levelConfig.LevelConfig.WallsCount));
            }
        }
    }
}