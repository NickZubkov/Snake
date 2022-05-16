using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<LevelController.Components.FoodSpawningSignal> _foodSignal;
        private EcsFilter<LevelController.Components.BonusSpawningSignal> _bonusSignal;
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            if (!_foodSignal.IsEmpty())
            {
                var spawnPosition = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
                var food = Object.Instantiate(_config.LevelsConfig[0].GoodsConfig.FoodPrefab, spawnPosition, Quaternion.identity);
                food.Spawn(_world.NewEntity(), _world);
            }
            
            if (!_bonusSignal.IsEmpty())
            {
                var spawnPosition = new Vector3(Random.Range(-19, 19), 0f, Random.Range(-19, 19));
                var bonus = Object.Instantiate(_config.LevelsConfig[0].GoodsConfig.BonusPrefab, spawnPosition, Quaternion.identity);
                bonus.Spawn(_world.NewEntity(), _world);
            }
        }
    }
}