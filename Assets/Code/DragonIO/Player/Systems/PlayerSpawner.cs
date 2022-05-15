using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerSpawner : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayerTag> _player;
        private EcsFilter<Dragons.Components.DragonHead, Components.PlayerSpawnedSignal> _spawnedSignal;
        
        private EcsWorld _world;
        private Data.GameConfig _config;

        public void Run()
        {
            if (_player.IsEmpty())
            {
                var position = new Vector3(0, 0.5f, 0);
                var player = Object.Instantiate(_config.DragonHeadPrefab, position, Quaternion.identity);
                player.Spawn(_world.NewEntity(), _world);
            }

            
            foreach (var idx in _spawnedSignal)
            {
                var position = new Vector3(0, 0.5f, 0);
                ref var signal = ref _spawnedSignal.Get1(idx);
                
                var bodyWithLegs = Object.Instantiate(_config.DragonBodyPrefabFrontLegs, position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                signal.BodyParts.Add(bodyWithLegs.transform);

                bodyWithLegs = Object.Instantiate(_config.DragonBodyPrefabBackLegs, position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                signal.BodyParts.Add(bodyWithLegs.transform);
            }
        }
    }
}