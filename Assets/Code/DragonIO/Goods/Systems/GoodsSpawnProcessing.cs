using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Goods.Systems
{
    public class GoodsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.GoodsSpawningSignal> _goodsSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        
        private EcsWorld _world;
        public void Run()
        {
            foreach (var goodsSignal in _goodsSignal)
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    foreach (var player in _player)
                    {
                        ref var playerTransform = ref _player.Get1(player).Transform;
                        var randomPoint = Random.insideUnitCircle * levelRunTimeData.ObjectsMaxSpawnRadius;
                        var prefab = _goodsSignal.Get1(goodsSignal).GoodsPrefab;
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                        if ((playerTransform.position - position).sqrMagnitude < levelRunTimeData.ObjectsMinSpawnRadiusSqr)
                        {
                            break;
                        }
                        var food = Object.Instantiate(prefab, position, Quaternion.identity);
                        food.Spawn(_world.NewEntity(), _world);
                    }
                }
            }
        }
    }
}