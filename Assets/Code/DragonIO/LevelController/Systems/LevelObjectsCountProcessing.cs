﻿using Leopotam.Ecs;
using Modules.Utils;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelObjectsCountProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.LevelRunTimeData, Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<Enemy.Components.Enemy> _enemies;
        private EcsFilter<Location.Components.Wall> _walls;
        private EcsFilter<Goods.Components.Food> _food;
        private EcsFilter<Goods.Components.Bonus> _bonus;
        private EcsFilter<Location.Components.GroundDecor> _groundDecor;
        private EcsFilter<Location.Components.Obstacle>.Exclude<Location.Components.Wall> _obstacle;
        private EcsFilter<Player.Components.Player> _player;
        private EcsFilter<Components.PLayerSpawnedTag> _pLayerSpawnedTag;
        private EcsFilter<Components.PlayerSpawningSignal> _pLayerSpawningSignal;
        
        
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Dragons.Components.DragonHead> _dragons;
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _playerHead;
        
        
        private EcsWorld _world;
        private TimeService _timeService;

        public void Run()
        {
            foreach (var idx in _levelData)
            {
                ref var levelRunTimeData = ref _levelData.Get1(idx);
                ref var levelConfigs = ref _levelData.Get2(idx);
                
                if (_walls.GetEntitiesCount() < Data.GameConstants.WALLS_COUNT)
                {
                    _levelData.GetEntity(idx).Get<Components.WallSpawningSignal>();
                }

                if (_enemies.GetEntitiesCount() < levelConfigs.LocationConfig.EnemiesCount)
                {
                    _levelData.GetEntity(idx).Get<Components.EnemySpawningSignal>();
                }

                if (_food.GetEntitiesCount() < levelRunTimeData.FoodCount)
                {
                    ref var signal = ref _world.NewEntity().Get<Components.GoodsSpawningSignal>();
                    signal.GoodsPrefab = levelConfigs.GoodsConfig.FoodPrefab;
                }

                foreach (var data in levelRunTimeData.FoodSpawningPositions)
                {
                    if (data.Count > 0)
                    {
                        ref var signal = ref _world.NewEntity().Get<Components.GoodsSpawningSignal>();
                        signal.GoodsPrefab = levelConfigs.GoodsConfig.FoodPrefab;
                        signal.SpawningPosition = data.Dequeue();
                        signal.UseBodyPosition = true;
                    }
                }

                foreach (var data in levelRunTimeData.FoodSpawningPositions)
                {
                    if (data.Count <= 0)
                    {
                        levelRunTimeData.FoodSpawningPositions.Dequeue();
                        break;
                    }
                }

                if (levelRunTimeData.BonusSpawnTimer <= 0)
                {
                    if (_bonus.GetEntitiesCount() < levelConfigs.GoodsConfig.MaxBonusCount)
                    {
                        var index = Random.Range(0, levelConfigs.GoodsConfig.BonusPrefabs.Count);
                        _world.NewEntity().Get<Components.GoodsSpawningSignal>().GoodsPrefab = levelConfigs.GoodsConfig.BonusPrefabs[index];
                    }
                    
                    levelRunTimeData.BonusSpawnTimer = Random.Range(levelConfigs.GoodsConfig.BonusSpawnTimeRange.x, (float)levelConfigs.GoodsConfig.BonusSpawnTimeRange.y);
                }
                
                if (_groundDecor.GetEntitiesCount() < levelConfigs.GroundConfig.GroundDecorCount)
                {
                    _levelData.GetEntity(idx).Get<Components.GroundDecorSpawningSignal>();
                }
                
                if (_obstacle.GetEntitiesCount() < levelConfigs.GroundConfig.ObstaclesCount)
                {
                    _levelData.GetEntity(idx).Get<Components.ObstaclesSpawningSignal>();
                }
                
                if (_player.IsEmpty() && _pLayerSpawnedTag.IsEmpty())
                {
                    _levelData.GetEntity(idx).Get<Components.PlayerSpawningSignal>();
                    _levelData.GetEntity(idx).Get<Components.PLayerSpawnedTag>();
                    levelRunTimeData.PlayerDethVibroPlayed = false;
                }

                if (_player.IsEmpty() && !_pLayerSpawnedTag.IsEmpty() && _pLayerSpawningSignal.IsEmpty())
                {
                    if (!levelRunTimeData.PlayerDethVibroPlayed)
                    {
                        Misc.PlayVibro(HapticTypes.Failure);
                        levelRunTimeData.PlayerDethVibroPlayed = true;
                    }
                    levelRunTimeData.WinFailTaimer -= _timeService.DeltaTime;
                    if (levelRunTimeData.WinFailTaimer <= 0)
                    {
                        _levelData.GetEntity(idx).Get<Components.LevelFaildSignal>();
                    }
                }

                foreach (var dragon in _dragons)
                {
                    ref var head = ref _dragons.Get1(dragon);
                    if (head.BodyParts != null && head.BodyParts.Count < head.StartBodyCount + 3)
                    {
                        ref var signal = ref _dragons.GetEntity(dragon).Get<Components.DragonBodySpawningSignal>();
                        signal.DragonHead = head;
                        signal.BodyPrefab = head.DragonConfig.BodyPrefab;
                    }
                }
                
                
                
                
                if (_gameplay.IsEmpty())
                    return;
                
                
                
                
                
                foreach (var player in _playerHead)
                {
                    
                    ref var playerHead = ref _playerHead.Get1(player);
                    
                    playerHead.LockDirectionTimer -= _timeService.DeltaTime;
                    
                    if (playerHead.LockDirectionTimer <= 0)
                        playerHead.LockDirection = false;
                    
                    levelRunTimeData.PlayerPoints = playerHead.Points;
                    
                    if (levelRunTimeData.LevelTimer > 0)
                        levelRunTimeData.LevelTimer -= _timeService.DeltaTime;

                    if (levelRunTimeData.LevelTimer <= 0)
                    {
                        levelRunTimeData.LevelTimer = 0;
                        
                        int maxPoints = -1;
                        foreach (var dragon in _dragons)
                        {
                            if (maxPoints < _dragons.Get1(dragon).Points)
                            {
                                maxPoints = _dragons.Get1(dragon).Points;
                            }
                        }

                        if (playerHead.Points < maxPoints)
                        {
                            EventGroup.StateFactory.CreateState<EventGroup.RoundFailedState>(_world);
                            Misc.PlayVibro(HapticTypes.Failure);
                        }
                        else
                        {
                            if (!playerHead.WinVFX.isPlaying)
                            {
                                _playerHead.GetEntity(idx).Get<Goods.Components.PlayWinVFXSignal>();
                                Misc.PlayVibro(HapticTypes.Success);
                            }
                                
                            playerHead.MovementSpeed = 0f;
                            playerHead.RotationSpeed = 0f;
                            levelRunTimeData.WinFailTaimer -= _timeService.DeltaTime;
                            
                            if (levelRunTimeData.WinFailTaimer <= 0)
                            {
                                _playerHead.GetEntity(idx).Get<Components.LevelComplitedSignal>();
                            }
                        }
                    }
                }
                
                
                
                levelRunTimeData.BonusSpawnTimer -= _timeService.DeltaTime;
                
                
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
                            dragon.Gap /= dragon.SpeedBonusMultiplyer;
                            _dragons.GetEntity(dragons).Get<Goods.Components.StopPowerUpVFXSignal>().BonusType = Goods.Components.BonusType.Speed;
                        }
                    }
                    if (dragon.ShieldBonusTimer > 0)
                    {
                        dragon.ShieldBonusTimer -= _timeService.DeltaTime;
                        if (dragon.ShieldBonusTimer <= 0)
                        {
                            dragon.IsShieldActive = false;
                            _dragons.GetEntity(dragons).Get<Goods.Components.StopPowerUpVFXSignal>().BonusType = Goods.Components.BonusType.Shield;
                        }
                    }
                    if (dragon.PointBonusTimer > 0)
                    {
                        dragon.PointBonusTimer -= _timeService.DeltaTime;
                        if (dragon.PointBonusTimer <= 0)
                        {
                            dragon.PointBonusMultiplyer = (int)dragon.DefaultBonusMultiplyer;
                            _dragons.GetEntity(dragons).Get<Goods.Components.StopPowerUpVFXSignal>().BonusType = Goods.Components.BonusType.Point;
                        }
                    }
                }

            }
        }
    }
}