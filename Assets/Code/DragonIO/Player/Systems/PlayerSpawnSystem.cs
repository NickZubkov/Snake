using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Dragons.Components.DragonHead, Components.Player> _player;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayerHeadSpawnedSignal> _spawnedSignal;
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
                    var player = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.HeadPrefab, Vector3.zero, Quaternion.identity);
                    player.Spawn(_world.NewEntity(), _world);
                    player.AddPlayerComponent(controller.LevelsConfigs.PlayerConfig);
                }
            }
            
            foreach (var spawnedSignal in _spawnedSignal)
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    ref var signal = ref _spawnedSignal.Get1(spawnedSignal);
                
                    var bodyWithLegs = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.BodyPrefabFrontLegs, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(spawnedSignal));
                    signal.BodyParts.Add(bodyWithLegs.transform);

                    bodyWithLegs = Object.Instantiate(controller.LevelsConfigs.PlayerConfig.BodyPrefabBackLegs, Vector3.zero, Quaternion.identity);
                    bodyWithLegs.Spawn(_world.NewEntity(), _world);
                    bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(spawnedSignal));
                    signal.BodyParts.Add(bodyWithLegs.transform);
                }
                
            }
        }
    }
}