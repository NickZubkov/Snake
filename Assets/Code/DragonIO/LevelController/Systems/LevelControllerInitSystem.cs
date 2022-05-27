using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using Modules.DragonIO.Dragons.Components;
using UnityEngine;

namespace Modules.DragonIO.LevelController.Systems
{
    public class LevelControllerInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.LevelController> _controller;
        private EcsFilter<CameraUtils.VirtualCamera> _virtualCamera;
        
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
                controller.WallSize = 150 * controller.LevelsConfigs.LocationConfig.LevelSize * Mathf.Sin(180 * Mathf.Deg2Rad / controller.LevelsConfigs.LocationConfig.WallsCount);
                controller.PlaceRadius = 74.5f * controller.LevelsConfigs.LocationConfig.LevelSize;//controller.WallSize / (2 * Mathf.Tan(180 * Mathf.Deg2Rad / controller.LevelsConfigs.LocationConfig.WallsCount));
                foreach (var camera in _virtualCamera)
                {
                    controller.CinemachineTransposer = _virtualCamera.Get1(camera)
                        .Camera
                        .GetComponent<Cinemachine.CinemachineVirtualCamera>()
                        .GetCinemachineComponent(Cinemachine.CinemachineCore.Stage.Body) as Cinemachine.CinemachineTransposer;
                    controller.CinemachineTransposer.m_FollowOffset = _config.DefaultCameraOffset;
                }

                controller.DragonScalingFactor = _config.DragonScalingFactor;
                controller.LevelTimer = controller.LevelsConfigs.LocationConfig.LevelTimer;
                controller.SpawnedEnemiesCount = 0;
                controller.ObjectsSpawnRadius = _config.ObjectsSpawnRadius;

            }
        }
    }
}