using Leopotam.Ecs;
using Modules.Utils;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        private EcsFilter<Goods.Components.Food> _food;
        private EcsFilter<Goods.Components.Bonus> _bonus;
        private EcsFilter<Dragons.Components.DragonHead> _dragons;
        private TimeService _timeService;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _controller)
            {
                ref var controller = ref _controller.Get1(idx);

                controller.BonusSpawnTimer -= _timeService.DeltaTime;
                
                if (_food.GetEntitiesCount() < controller.LevelsConfigs.GoodsConfig.MinFoodCount)
                {
                    _controller.GetEntity(idx).Get<Components.FoodSpawningSignal>();
                }
                
                if (controller.BonusSpawnTimer <= 0)
                {
                    if (_bonus.GetEntitiesCount() < controller.LevelsConfigs.GoodsConfig.MaxBonusCount)
                    {
                        _controller.GetEntity(idx).Get<Components.BonusSpawningSignal>();
                    }
                    
                    controller.BonusSpawnTimer = Random.Range(
                        controller.LevelsConfigs.GoodsConfig.BonusSpawnTimeRange.x, 
                        controller.LevelsConfigs.GoodsConfig.BonusSpawnTimeRange.y);
                }

                foreach (var dragons in _dragons)
                {
                    ref var dragon = ref _dragons.Get1(dragons);
                    if (dragon.SpeedBonusTimer > 0)
                    {
                        
                        dragon.SpeedBonusTimer -= _timeService.DeltaTime;
                        if (dragon.SpeedBonusTimer <= 0)
                        {
                            dragon.MovementSpeed /= dragon.SpeedBonusMultiplyer;
                        }
                    }
                    if (dragon.ShieldBonusTimer > 0)
                    {
                        dragon.ShieldBonusTimer -= _timeService.DeltaTime;
                        if (dragon.ShieldBonusTimer <= 0)
                        {
                            dragon.IsShieldActive = false;
                        }
                    }
                    if (dragon.PointBonusTimer > 0)
                    {
                        
                        dragon.PointBonusTimer -= _timeService.DeltaTime;
                        if (dragon.PointBonusTimer <= 0)
                        {
                            dragon.PointBonusMultiplyer = (int)dragon.DefaultMultiplyer;
                        }
                    }
                }
            }
        }
    }
}