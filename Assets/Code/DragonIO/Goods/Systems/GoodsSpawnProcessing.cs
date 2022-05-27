using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<LevelController.Components.FoodSpawningSignal> _foodSignal;
        private EcsFilter<LevelController.Components.BonusSpawningSignal> _bonusSignal;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        
        private EcsWorld _world;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            if (!_foodSignal.IsEmpty())
            {
                foreach (var idx in _levelController)
                {

                    foreach (var player in _player)
                    {
                        ref var controller = ref _levelController.Get1(idx);
                        ref var playerTransform = ref _player.Get1(player).Transform;
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var prefab = controller.LevelsConfigs.GoodsConfig.FoodPrefab;
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        while ((playerTransform.position - position).sqrMagnitude < controller.ObjectsSpawnRadius * controller.ObjectsSpawnRadius)
                        {
                            randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                            position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        }
                        var food = Object.Instantiate(prefab, position, Quaternion.identity);
                        food.Spawn(_world.NewEntity(), _world);
                        controller.GoodsPositions.Insert(0, food.transform);
                    }
                }
            }
            
            if (!_bonusSignal.IsEmpty())
            {
                foreach (var idx in _levelController)
                {
                    foreach (var player in _player)
                    {
                        ref var controller = ref _levelController.Get1(idx);
                        ref var playerTransform = ref _player.Get1(player).Transform;
                        var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                        var index = Random.Range(0, controller.LevelsConfigs.GoodsConfig.BonusPrefabs.Count);
                        var prefab = controller.LevelsConfigs.GoodsConfig.BonusPrefabs[index];
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        while ((playerTransform.position - position).magnitude < controller.ObjectsSpawnRadius)
                        {
                            randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                            position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        }
                        var bonus = Object.Instantiate(prefab, position, Quaternion.identity);
                        bonus.Spawn(_world.NewEntity(), _world);
                        controller.GoodsPositions.Insert(0, bonus.transform);
                    }
                }
            }
        }
    }
}