using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerInitSystem : IEcsRunSystem
    {
        private EcsFilter<Components.LevelRunTimeData, Components.CurrentLevelConfigs> _levelData;
        readonly EcsFilter<LevelSpawner.LevelSpawnedSignal> _levelSpawned;
        private EcsFilter<CameraUtils.VirtualCamera> _virtualCamera;
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        
        public void Run()
        {
            if (_levelSpawned.IsEmpty())
                return;
            
            if (_levelData.IsEmpty())
            {
                var entity = _world.NewEntity();
                ref var levelRunTimeData = ref entity.Get<Components.LevelRunTimeData>();
                ref var levelConfigs = ref entity.Get<Components.CurrentLevelConfigs>();
                entity.Get<LevelSpawner.LevelEntityTag>();
                
                var currentLevelID = _config.LevelsConfigs.Keys.ToArray().SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel);
                levelConfigs.LocationConfig = _config.LevelsConfigs[currentLevelID].LocationConfig;
                levelConfigs.EnemiesConfigs = _config.LevelsConfigs[currentLevelID].EnemiesConfigs;
                levelConfigs.GoodsConfig = _config.LevelsConfigs[currentLevelID].GoodsConfig;
                levelConfigs.GroundConfig = _config.LevelsConfigs[currentLevelID].GroundConfig;
                levelConfigs.PlayerConfig = _config.LevelsConfigs[currentLevelID].PlayerConfig;
                
                levelRunTimeData.BonusSpawnTimer = Random.Range(levelConfigs.GoodsConfig.BonusSpawnTimeRange.x, levelConfigs.GoodsConfig.BonusSpawnTimeRange.y);
                levelRunTimeData.WallSize = Data.GameConstants.LEVEL_PREFAB_SIZE * levelConfigs.LocationConfig.LevelSize * Mathf.Sin(180 * Mathf.Deg2Rad / Data.GameConstants.WALLS_COUNT);
                levelRunTimeData.WallMaxSpawnRadius = Data.GameConstants.WALL_SPAWN_RADIUS * levelConfigs.LocationConfig.LevelSize;
                levelRunTimeData.GroundDecorMaxSpawnRadius = Data.GameConstants.GROUND_DECOR_SPAWN_RADIUS * levelConfigs.LocationConfig.LevelSize;
                levelRunTimeData.OtherObjectMaxSpawnRadius = Data.GameConstants.OTHER_OBJECTS_SPAWN_RADIUS * levelConfigs.LocationConfig.LevelSize;
                levelRunTimeData.DragonScalingFactor = _config.DragonScalingFactor;
                levelRunTimeData.LevelTimer = levelConfigs.LocationConfig.LevelTimer;
                levelRunTimeData.WinFailTaimer = 2f;
                levelRunTimeData.SpawnedEnemiesCount = 0;
                levelRunTimeData.EnemyMinSpawnRadiusSqr = _config.EnemyMinSpawnRadius * _config.EnemyMinSpawnRadius;
                levelRunTimeData.ObstaclesMinSpawnRadiusSqr = _config.ObstaclesMinSpawnRadius * _config.ObstaclesMinSpawnRadius;
                levelRunTimeData.GoodsMinSpawnRadiusSqr = _config.GoodsMinSpawnRadius * _config.GoodsMinSpawnRadius;
                levelRunTimeData.GoodsLayerMask = 1 << 6;
                levelRunTimeData.GoodsCollectingRadius = _config.GoodsCollectingRadius;
                levelRunTimeData.MaxGoodsSerchingCount = _config.MaxGoodsSerchingCount;
                levelRunTimeData.FoodCount = levelConfigs.GoodsConfig.MinFoodCount;
                levelRunTimeData.FoodSpawningPositions = new Queue<Queue<Vector3>>();
                levelRunTimeData.BodyPartSpawnDecrease = _config.BodyPartSpawnDecrease;
                levelRunTimeData.CameraOffset = _config.CameraOffset;

                foreach (var camera in _virtualCamera)
                {
                    levelRunTimeData.CinemachineTransposer = _virtualCamera.Get1(camera)
                        .Camera
                        .GetComponent<Cinemachine.CinemachineVirtualCamera>()
                        .GetCinemachineComponent(Cinemachine.CinemachineCore.Stage.Body) as Cinemachine.CinemachineTransposer;
                    levelRunTimeData.CinemachineTransposer.m_FollowOffset = _config.DefaultCameraOffset;
                }
            }
        }
    }
}