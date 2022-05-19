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
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            if (!_foodSignal.IsEmpty())
            {
                foreach (var idx in _levelController)
                {
                    ref var controller = ref _levelController.Get1(idx);
                    var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                    var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                    var prefab = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).GoodsConfig.FoodPrefab;
                    var food = Object.Instantiate(prefab, position, Quaternion.identity);
                    food.Spawn(_world.NewEntity(), _world);
                    controller.GoodsPositions.Insert(0, food.transform);
                }
            }
            
            if (!_bonusSignal.IsEmpty())
            {
                foreach (var idx in _levelController)
                { 
                    ref var controller = ref _levelController.Get1(idx);
                    var randomPoint = Random.insideUnitCircle * controller.PlaceRadius;
                    var position = new Vector3(randomPoint.x, 0f, randomPoint.y);
                    var prefab = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).GoodsConfig.BonusPrefab;
                    var bonus = Object.Instantiate(prefab, position, Quaternion.identity);
                    bonus.Spawn(_world.NewEntity(), _world);
                    controller.GoodsPositions.Insert(0, bonus.transform);
                }
            }
        }
    }
}