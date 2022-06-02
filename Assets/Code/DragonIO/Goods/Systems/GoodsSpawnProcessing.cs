﻿using Leopotam.Ecs;
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
                    ref var signal = ref _goodsSignal.Get1(goodsSignal);
                    foreach (var player in _player)
                    {
                        var position = Vector3.zero;
                        var prefab = _goodsSignal.Get1(goodsSignal).GoodsPrefab;
                        
                        if (signal.UseBodyPosition)
                        {
                            position = signal.SpawningPosition;
                        }
                        else
                        {
                            ref var playerTransform = ref _player.Get1(player).Transform;
                            var randomPoint = Random.insideUnitCircle * levelRunTimeData.OtherObjectMaxSpawnRadius;
                            position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);
                            if ((playerTransform.position - position).sqrMagnitude < levelRunTimeData.GoodsMinSpawnRadiusSqr)
                            {
                                break;
                            }
                        }
                        
                        var goods = Object.Instantiate(prefab, position, Quaternion.identity);
                        goods.Spawn(_world.NewEntity(), _world);
                    }
                }
            }
        }
    }
}