using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.GoodsSpawningSignal> _goodsSignal;
        private EcsFilter<Components.PooledFoodTag> _foodPool;

        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs>
            _levelData;

        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;

        private EcsWorld _world;

        public void Run()
        {
            foreach (var goodsSignal in _goodsSignal)
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var signal = ref _goodsSignal.Get1(goodsSignal);
                    var playerPosition = Vector3.zero;
                    
                    foreach (var player in _player)
                    {
                        playerPosition = _player.Get1(player).Transform.position;
                    }
                    
                    var position = Vector3.zero;
                    var prefab = _goodsSignal.Get1(goodsSignal).GoodsPrefab;

                    if (signal.UseBodyPosition)
                    {
                        position = signal.SpawningPosition;
                    }
                    else
                    {
                        var randomPoint = Random.insideUnitCircle * levelRunTimeData.OtherObjectMaxSpawnRadius;
                        position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        if ((playerPosition - position).sqrMagnitude < levelRunTimeData.GoodsMinSpawnRadiusSqr)
                        {
                            break;
                        }
                    }

                    if (_foodPool.IsEmpty())
                    {
                        var goods = Object.Instantiate(prefab, position, Quaternion.identity);
                        goods.Spawn(_world.NewEntity(), _world);
                    }
                    else
                    {
                        foreach (var foodPool in _foodPool)
                        {
                            ref var food = ref _foodPool.GetEntity(foodPool);
                            food.Del<Components.PooledFoodTag>();
                            food.Get<Components.Food>();
                            food.Get<ViewHub.UnityView>().Transform.position = position;
                            food.Get<ViewHub.UnityView>().Transform.localScale = Vector3.one;
                            food.Get<ViewHub.UnityView>().GameObject.SetActive(true);
                            break;
                        }
                    }
                }
            }
        }
    }
}