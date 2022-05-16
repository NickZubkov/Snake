using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawnSystem : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayerTag> _player;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayerSpawnedSignal> _spawnedSignal;
        
        private EcsWorld _world;
        private Data.GameConfig _config;

        public void Run()
        {
            if (_player.IsEmpty())
            {
                var dragonConfig = _config.LevelsConfig[0].PlayerConfig;
                var player = Object.Instantiate(dragonConfig.DragonHeadPrefab, Vector3.zero, Quaternion.identity);
                player.Spawn(_world.NewEntity(), _world);
            }
            
            foreach (var idx in _spawnedSignal)
            {
                ref var signal = ref _spawnedSignal.Get1(idx);
                var dragonConfig = _config.LevelsConfig[0].PlayerConfig;
                
                var bodyWithLegs = Object.Instantiate(dragonConfig.DragonBodyPrefabFrontLegs, Vector3.zero, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                signal.BodyParts.Add(bodyWithLegs.transform);

                bodyWithLegs = Object.Instantiate(dragonConfig.DragonBodyPrefabBackLegs, Vector3.zero, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                signal.BodyParts.Add(bodyWithLegs.transform);
            }
        }
    }
}