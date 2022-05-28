using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.PlayerSpawningSignal> _playerSpawningSignal;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.PlayerHeadSpawnedSignal> _playerHeadSpawnedSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        
        private EcsWorld _world;

        public void Run()
        {
            if (!_playerSpawningSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    var parent = new GameObject("Player");
                    var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                    parentEntity._components = new List<ViewHub.ViewComponent>();
                    parentEntity.Spawn(_world.NewEntity(), _world);
                    var player = Object.Instantiate(currentLevelConfigs.PlayerConfig.HeadPrefab, Vector3.zero, Quaternion.identity);
                    player.Spawn(_world.NewEntity(), _world);
                    player.transform.parent = parent.transform;
                    player.AddPlayerComponent(currentLevelConfigs.PlayerConfig);
                    
                }
            }
            
            foreach (var spawnedSignal in _playerHeadSpawnedSignal)
            {
                foreach (var levelData in _levelData)
                {
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    ref var signal = ref _playerHeadSpawnedSignal.Get2(spawnedSignal);
                    ref var dragonHeadTransform = ref _playerHeadSpawnedSignal.Get1(spawnedSignal).Transform;
                
                    var bodyWithLegs = Object.Instantiate(currentLevelConfigs.PlayerConfig.LegsPrefab, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_playerHeadSpawnedSignal.GetEntity(spawnedSignal));
                    bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                    signal.BodyParts.Add(bodyWithLegs.transform);

                    bodyWithLegs = Object.Instantiate(currentLevelConfigs.PlayerConfig.TailPrefab, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_playerHeadSpawnedSignal.GetEntity(spawnedSignal));
                    bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                    signal.BodyParts.Add(bodyWithLegs.transform);
                }
                
            }
        }
    }
}