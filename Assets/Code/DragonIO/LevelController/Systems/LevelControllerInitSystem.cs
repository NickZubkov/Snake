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
            {
                foreach (var idx in _enemiesSpawnedTag)
                {
                    _enemiesSpawnedTag.GetEntity(idx).Del<Enemy.Components.EnemiesSpawnedTag>();
                }
                return;
            }
                
            
            if (_controller.IsEmpty())
            {
                ref var entity = ref _world.NewEntity().Get<Components.LevelController>();
                var levelConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel);
                entity.LevelTimer = levelConfig.LevelTimer;
                entity.MinFoodCount = levelConfig.GoodsConfig.MinFoodCount;
                entity.MaxBonusCount = levelConfig.GoodsConfig.MaxBonusCount;
                entity.BonusMinSpawnTime = levelConfig.GoodsConfig.BonusSpawnTimeRange.Min;
                entity.BonusMaxSpawnTime = levelConfig.GoodsConfig.BonusSpawnTimeRange.Max;
                entity.BonusSpawnTimer = Random.Range(entity.BonusMinSpawnTime, entity.BonusMaxSpawnTime);
            }
        }
    }
}