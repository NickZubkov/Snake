using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<Components.DragonHead, UPhysics.Triggered> _dragon;
        
        private EcsWorld _world;
        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;

            foreach (var idx in _dragon)
            {
                ref var triggered = ref _dragon.Get2(idx);
                
                if (triggered.Other.Has<Goods.Components.Food>())
                {
                    ref var dragonHead = ref _dragon.Get1(idx);
                    var bodyPart = Object.Instantiate(dragonHead.Config.DragonBodyPrefab, dragonHead.BodyParts[1].position, Quaternion.identity);
                    bodyPart.Spawn(_world.NewEntity(), _world);
                    dragonHead.BodyParts.Insert(2, bodyPart.transform);
                    dragonHead.Points++;
                    triggered.Other.Destroy();
                }
                else if (triggered.Other.Has<Goods.Components.Bonus>())
                {
                    triggered.Other.Destroy();
                }
            }
        }
    }
}