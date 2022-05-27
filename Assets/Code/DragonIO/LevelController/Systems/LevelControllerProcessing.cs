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
        private EcsFilter<Components.ChangeCameraOffsetSignal> _cameraOffsetSignal;
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _player;

        private EcsWorld _world;
        private TimeService _timeService;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _controller)
            {
                ref var controller = ref _controller.Get1(idx);

                controller.LevelTimer -= _timeService.DeltaTime;
                
                foreach (var player in _player)
                {
                    var playerPoints = _player.Get1(player).Points;
                    controller.PlayerPoints = playerPoints;

                    if (controller.LevelTimer <= 0)
                    {
                        int maxPoints = -1;
                        foreach (var dragon in _dragons)
                        {
                            if (maxPoints < _dragons.Get1(dragon).Points)
                            {
                                maxPoints = _dragons.Get1(dragon).Points;
                            }
                        }

                        if (playerPoints < maxPoints)
                        {
                            EventGroup.StateFactory.CreateState<EventGroup.RoundFailedState>(_world);
                        }
                        else
                        {
                            EventGroup.StateFactory.CreateState<EventGroup.RoundCompletedState>(_world);
                        }
                    }
                }
                
                if (_food.GetEntitiesCount() < controller.LevelsConfigs.GoodsConfig.MinFoodCount)
                {
                    _controller.GetEntity(idx).Get<Components.FoodSpawningSignal>();
                }
                
                controller.BonusSpawnTimer -= _timeService.DeltaTime;
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
                // bonus timers
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

                foreach (var signal in _cameraOffsetSignal)
                {
                    controller.CinemachineTransposer.m_FollowOffset += new Vector3(0, controller.DragonScalingFactor, 0);
                    _cameraOffsetSignal.GetEntity(signal).Del<Components.ChangeCameraOffsetSignal>();  
                }
                
            }
        }
    }
}