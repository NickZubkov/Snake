using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<ViewHub.UnityView , Components.DragonHead, UPhysics.Triggered> _dragon;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        
        private EcsWorld _world;
        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;

            foreach (var idx in _dragon)
            {
                ref var triggered = ref _dragon.Get3(idx);
               
                if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Food>())
                {
                    ref var dragonHead = ref _dragon.Get2(idx);
                    var index = dragonHead.BodyParts.Count - 1;
                    var bodyPart = Object.Instantiate(dragonHead.DragonConfig.BodyPrefab, dragonHead.BodyParts[index].position, Quaternion.identity);
                    bodyPart.Spawn(_world.NewEntity(), _world);
                    bodyPart.SetComponentReferences(_dragon.GetEntity(idx));
                    bodyPart.transform.parent = _dragon.Get1(idx).Transform.parent;
                    dragonHead.BodyParts.Insert(index, bodyPart.transform);
                    dragonHead.Points++;
                    foreach (var levelController in _levelController)
                    {
                        for (int i = 0; i < _levelController.Get1(levelController).GoodsPositions.Count; i++)
                        {
                            if (_levelController.Get1(levelController).GoodsPositions[i].gameObject == triggered.Collider.transform.parent.parent.gameObject)
                            {
                                _levelController.Get1(levelController).GoodsPositions.RemoveAt(i);
                            } 
                        }
                    }
                    triggered.Other.Destroy();
                }
                else if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Bonus>())
                {
                    foreach (var levelController in _levelController)
                    {
                        for (int i = 0; i < _levelController.Get1(levelController).GoodsPositions.Count; i++)
                        {
                            if (_levelController.Get1(levelController).GoodsPositions[i].gameObject == triggered.Collider.transform.parent.parent.gameObject)
                            {
                                _levelController.Get1(levelController).GoodsPositions.RemoveAt(i);
                            } 
                        }
                    }
                    triggered.Other.Destroy();
                }
            }
        }
    }
}