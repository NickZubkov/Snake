using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemySpawnSystem : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.EnemySpawningSignal> _enemySpawningSignal;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy, Components.EnemyHeadSpawnedSignal> _spawnedSignal;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        
        private EcsWorld _world;

        public void Run()
        {
            if(_enemySpawningSignal.IsEmpty())
                return;

            foreach (var levelData in _levelData)
            {
                foreach (var player in _player)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    ref var playerTransform = ref _player.Get1(player).Transform;

                    var index = Random.Range(0, currentLevelConfigs.EnemiesConfigs.Count);
                    var dragonConfigs = currentLevelConfigs.EnemiesConfigs[index];
                    var randomPoint = Random.insideUnitCircle * levelRunTimeData.ObjectsMaxSpawnRadius;
                    var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                    if ((playerTransform.position - position).sqrMagnitude < levelRunTimeData.ObjectsMinSpawnRadiusSqr)
                    {
                        break;
                    }

                    var parent = new GameObject("Dragon_" + levelRunTimeData.SpawnedEnemiesCount);
                    var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                    parentEntity._components = new List<ViewHub.ViewComponent>();
                    parentEntity.Spawn(_world.NewEntity(), _world);
                    var enemy = Object.Instantiate(dragonConfigs.HeadPrefab, position, Quaternion.identity);
                    enemy.transform.parent = parent.transform;
                    enemy.Spawn(_world.NewEntity(), _world);
                    enemy.AddEnemyComponent(dragonConfigs);
                    levelRunTimeData.SpawnedEnemiesCount++;
                }
            }

            foreach (var signal in _spawnedSignal)
            {
                ref var dragonHead = ref _spawnedSignal.Get2(signal);
                ref var enemy = ref _spawnedSignal.Get3(signal);
                var dragonHeadTransform = _spawnedSignal.Get1(signal).Transform;

                var bodyWithLegs = Object.Instantiate(enemy.EnemyConfig.LegsPrefab, dragonHeadTransform.position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(signal));
                bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                dragonHead.BodyParts.Add(bodyWithLegs.transform);

                bodyWithLegs = Object.Instantiate(enemy.EnemyConfig.TailPrefab, dragonHeadTransform.position, Quaternion.identity);
                bodyWithLegs.Spawn(_world.NewEntity(), _world);
                bodyWithLegs.SetComponentReferences(_spawnedSignal.GetEntity(signal));
                bodyWithLegs.transform.parent = dragonHeadTransform.parent;
                dragonHead.BodyParts.Add(bodyWithLegs.transform);
            }
        }
    }
}