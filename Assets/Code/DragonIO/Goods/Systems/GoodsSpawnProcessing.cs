using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.FoodSpawningSignal> _foodSignal;
        private EcsFilter<LevelController.Components.BonusSpawningSignal> _bonusSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        
        private EcsWorld _world;
        public void Run()
        {
            if (!_foodSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    foreach (var player in _player)
                    {
                        ref var playerTransform = ref _player.Get1(player).Transform;
                        var randomPoint = Random.insideUnitCircle * levelRunTimeData.ObjectsMaxSpawnRadius;
                        var prefab = currentLevelConfigs.GoodsConfig.FoodPrefab;
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        if ((playerTransform.position - position).sqrMagnitude < levelRunTimeData.ObjectsMinSpawnRadiusSqr)
                        {
                            break;
                        }
                        var food = Object.Instantiate(prefab, position, Quaternion.identity);
                        food.Spawn(_world.NewEntity(), _world);
                        levelRunTimeData.GoodsPositions.Insert(0, food.transform);
                    }
                }
            }
            
            if (!_bonusSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    foreach (var player in _player)
                    {
                        ref var playerTransform = ref _player.Get1(player).Transform;
                        var randomPoint = Random.insideUnitCircle * levelRunTimeData.ObjectsMaxSpawnRadius;
                        var index = Random.Range(0, currentLevelConfigs.GoodsConfig.BonusPrefabs.Count);
                        var prefab = currentLevelConfigs.GoodsConfig.BonusPrefabs[index];
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        if ((playerTransform.position - position).sqrMagnitude < levelRunTimeData.ObjectsMinSpawnRadiusSqr)
                        {
                            break;
                        }
                        var bonus = Object.Instantiate(prefab, position, Quaternion.identity);
                        bonus.Spawn(_world.NewEntity(), _world);
                        levelRunTimeData.GoodsPositions.Insert(0, bonus.transform);
                    }
                }
            }
        }
    }
}