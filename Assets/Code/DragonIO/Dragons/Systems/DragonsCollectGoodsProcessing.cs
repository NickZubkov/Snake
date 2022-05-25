using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<ViewHub.UnityView, Components.DragonHead, UPhysics.Triggered> _dragon;
        private EcsFilter<LevelController.Components.LevelController> _levelController;

        private EcsWorld _world;

        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;

            foreach (var idx in _dragon)
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    ref var triggered = ref _dragon.Get3(idx);
                    ref var dragonHead = ref _dragon.Get2(idx);
                    ref var dragonTransform = ref _dragon.Get1(idx);
                    ref var dragonEntity = ref _dragon.GetEntity(idx);
                    if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Food>())
                    {
                        var index = dragonHead.BodyParts.Count - 1;
                        var bodyPart = Object.Instantiate(dragonHead.DragonConfig.BodyPrefab, dragonHead.BodyParts[index].position, Quaternion.identity);
                        bodyPart.Spawn(_world.NewEntity(), _world);
                        bodyPart.SetComponentReferences(dragonEntity);
                        bodyPart.transform.parent = dragonTransform.Transform.parent;
                        dragonHead.BodyParts.Insert(index, bodyPart.transform);
                        dragonHead.Points += dragonHead.PointBonusMultiplyer;

                        var newScale = dragonTransform.Transform.localScale + (Vector3.one * controller.DragonScalingFactor);
                        foreach (var part in dragonHead.BodyParts)
                        {
                            part.localScale = newScale;
                            var newPosition = new Vector3(part.position.x, part.position.y + controller.DragonScalingFactor, part.position.z);
                            part.position = newPosition;
                        }

                        if (dragonEntity.Has<Player.Components.Player>())
                        {
                            dragonEntity.Get<LevelController.Components.ChangeCameraOffsetSignal>();
                        }
                        
                        for (int i = 0; i < controller.GoodsPositions.Count; i++)
                        {
                            if (controller.GoodsPositions[i].gameObject == triggered.Collider.transform.parent.parent.gameObject)
                            {
                                controller.GoodsPositions.RemoveAt(i);
                            }
                        }

                        triggered.Other.Destroy();
                    }
                    else if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Bonus>())
                    {
                        for (int i = 0; i < controller.GoodsPositions.Count; i++)
                        {
                            if (controller.GoodsPositions[i].gameObject == triggered.Collider.transform.parent.parent.gameObject)
                            {
                                controller.GoodsPositions.RemoveAt(i);
                            }
                        }

                        triggered.Other.Get<Goods.Components.Bonus>().BonusApplyer.Activate(ref dragonHead);
                        triggered.Other.Destroy();
                    }
                }
            }
        }
    }
}