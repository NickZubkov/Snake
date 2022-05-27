using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Dragons.Components.DragonHead, Components.Player> _player;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.PlayerHeadSpawnedSignal> _spawnedSignal;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        
        private EcsWorld _world;

        public void Run()
        {
            if(_gameplay.IsEmpty())
                return;
            
            if (_player.IsEmpty())
            {
                foreach (var idx in _levelController)
                {
                    ref var controller = ref _levelController.Get1(idx);
                    var parent = new GameObject("Player");
                    var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                    parentEntity._components = new List<ViewHub.ViewComponent>();
                    parentEntity.Spawn(_world.NewEntity(), _world);
                    var player = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.HeadPrefab, Vector3.zero, Quaternion.identity);
                    player.Spawn(_world.NewEntity(), _world);
                    player.transform.parent = parent.transform;
                    player.AddPlayerComponent(controller.LevelsConfigs.PlayerConfig);
                    
                }
            }
            
            foreach (var spawnedSignal in _spawnedSignal)
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    ref var signal = ref _spawnedSignal.Get2(spawnedSignal);
                    ref var dragonHeadTransform = ref _spawnedSignal.Get1(spawnedSignal).Transform;
                
                    var bodyWithLegs = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.LegsPrefab, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(spawnedSignal));
                    bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                    signal.BodyParts.Add(bodyWithLegs.transform);

                    bodyWithLegs = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.TailPrefab, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(spawnedSignal));
                    bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                    signal.BodyParts.Add(bodyWithLegs.transform);
                }
                
            }
        }
    }
}