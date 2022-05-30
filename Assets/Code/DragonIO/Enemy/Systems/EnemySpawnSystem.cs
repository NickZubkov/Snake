using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemySpawnSystem : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.EnemySpawningSignal> _enemySpawningSignal;
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

                    var index = Random.Range(0, currentLevelConfigs.EnemiesConfigs.Count);
                    var enemyConfig = currentLevelConfigs.EnemiesConfigs[index];
                    var randomPoint = Random.insideUnitCircle * levelRunTimeData.OtherObjectMaxSpawnRadius;
                    var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                    if ((_player.Get1(player).Transform.position - position).sqrMagnitude < levelRunTimeData.EnemyMinSpawnRadiusSqr)
                    {
                        break;
                    }

                    var parent = new GameObject("Dragon_" + levelRunTimeData.SpawnedEnemiesCount);
                    var parentEntity = parent.AddComponent<Dragons.EntityTemplates.DragonParentTemplate>();
                    parentEntity._components = new List<ViewHub.ViewComponent>();
                    parentEntity.Spawn(_world.NewEntity(), _world);
                    
                    var enemyHeadTemplate = Object.Instantiate(enemyConfig.HeadPrefab, position, Quaternion.identity);
                    enemyHeadTemplate.transform.parent = parent.transform;
                    
                    var dragonHeadEntity = _world.NewEntity();
                    ref var dragonHead = ref dragonHeadEntity.Get<Dragons.Components.DragonHead>();
                    dragonHead.DragonConfig = enemyConfig;
                    dragonHead.HeadID = levelRunTimeData.SpawnedEnemiesCount;
                    InitEnemy(ref dragonHeadEntity, enemyConfig, currentLevelConfigs);
                    enemyHeadTemplate.Spawn(dragonHeadEntity, _world);

                    _world.NewEntity().Get<Dragons.Components.DragonHeadSpawnedSignal>().DragonHead = dragonHead;
                    
                    levelRunTimeData.SpawnedEnemiesCount++;
                }
            }
        }
        public void InitEnemy(ref EcsEntity dragonHeadEntity ,Data.EnemyConfig enemyConfig, LevelController.Components.CurrentLevelConfigs currentLevelConfigs)
        {
            ref var enemy = ref dragonHeadEntity.Get<Enemy.Components.Enemy>();
            enemy.EnemyAI = enemyConfig.EnemyAI;
            enemy.TimeToChangeDirection = enemyConfig.TimeToChangeDirection;
            enemy.ChangeDirectionTimer = 0;
            enemy.GoodsSerchRadius = enemyConfig.GoodsSerchRadius;
            enemy.MaxGoodsSerchingCount = (currentLevelConfigs.GoodsConfig.MinFoodCount + currentLevelConfigs.GoodsConfig.MaxBonusCount) / 10;
            enemy.ObstacleSerchingDistance = enemyConfig.ObstacleSerchingDistance;
            enemy.ObstacleLayerMask = 1 << 7;
            enemy.GoodslayerMask = 1 << 6;
            enemy.IsAvoidingObstacle = false;
        }
    }
}