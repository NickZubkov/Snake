﻿using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemySpawnSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState>.Exclude<EventGroup.StateEnter> _gameplay;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy, Components.EnemyHeadSpawnedSignal> _spawnedSignal;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<LevelController.Components.LevelController> _controller;
        
        private EcsWorld _world;

        public void Run()
        {
            if(_gameplay.IsEmpty())
                return;

            foreach (var idx in _controller)
            {
                foreach (var player in _player)
                {
                    ref var controller = ref _controller.Get1(idx);
                    ref var playerTransform = ref _player.Get1(player).Transform;
                
                    var locationConfig = controller.LevelsConfigs.LocationConfig;
                    var index = Random.Range(0, controller.LevelsConfigs.EnemiesConfigs.Count);
                    var dragonConfigs = controller.LevelsConfigs.EnemiesConfigs[index];
                
                    if(_enemy.GetEntitiesCount() < locationConfig.EnemiesCount)
                    {
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                        while ((playerTransform.position - position).sqrMagnitude < controller.ObjectsSpawnRadius * controller.ObjectsSpawnRadius)
                        {
                            position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                        }
                        var parent = new GameObject("Dragon_" + controller.SpawnedEnemiesCount);
                        var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                        parentEntity._components = new List<ViewHub.ViewComponent>();
                        parentEntity.Spawn(_world.NewEntity(), _world);
                        var enemy = Object.Instantiate(dragonConfigs.HeadPrefab, position, Quaternion.identity);
                        enemy.transform.parent = parent.transform;
                        enemy.Spawn(_world.NewEntity(), _world);
                        enemy.AddEnemyComponent(dragonConfigs);
                        controller.SpawnedEnemiesCount++;
                    }
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