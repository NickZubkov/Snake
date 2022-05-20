using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        
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
                var currentLevelID = _config.LevelsConfigs.Keys.ToArray().SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel);
                controller.LevelsConfigs = _config.LevelsConfigs[currentLevelID];
                controller.BonusSpawnTimer = Random.Range(controller.LevelsConfigs.GoodsConfig.BonusSpawnTimeRange.x, controller.LevelsConfigs.GoodsConfig.BonusSpawnTimeRange.y);
                controller.GoodsPositions = new List<Transform>();
                controller.WallSize = controller.LevelsConfigs.LocationConfig.LevelSize * Mathf.Sin(180 * Mathf.Deg2Rad / controller.LevelsConfigs.LocationConfig.WallsCount);
                controller.PlaceRadius = controller.WallSize / (2 * Mathf.Tan(180 * Mathf.Deg2Rad / controller.LevelsConfigs.LocationConfig.WallsCount));
                
            }
        }
    }
}