using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemySpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.EnemyHeadSpawnedSignal> _spawnedSignal;
        private EcsFilter<Components.EnemiesSpawnedTag> _enemiesSpawnedTag;
        private EcsFilter<LevelController.Components.LevelController> _controller;
        
        private EcsWorld _world;
        private Data.GameConfig _config;

        public void Run()
        {
            if(_gameplay.IsEmpty())
                return;

            if (_enemy.IsEmpty() && _enemiesSpawnedTag.IsEmpty())
            {
                var dragonConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).EnemyConfig;

                for (int i = 0; i < dragonConfig.EnemyCount; i++)
                {
                    var position = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
                    var enemy = Object.Instantiate(dragonConfig.DragonHeadPrefab, position, Quaternion.identity);
                    enemy.Spawn(_world.NewEntity(), _world);
                }

                foreach (var idx in _controller)
                {
                    _controller.GetEntity(idx).Get<Components.EnemiesSpawnedTag>();
                }
            }

            foreach (var signal in _spawnedSignal)
            {
                ref var dragonHead = ref _spawnedSignal.Get2(signal);

                var position = _spawnedSignal.Get1(signal).Transform.position;
                var dragonConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).EnemyConfig;

                var bodyWithLegs = Object.Instantiate(dragonConfig.DragonBodyPrefabFrontLegs, position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(signal));
                dragonHead.BodyParts.Add(bodyWithLegs.transform);

                bodyWithLegs = Object.Instantiate(dragonConfig.DragonBodyPrefabBackLegs, position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(signal));
                dragonHead.BodyParts.Add(bodyWithLegs.transform); 
                
            }
        }
    }
}