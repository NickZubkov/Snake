using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollisionsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.DragonHead, UPhysics.Triggered> _dragons;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        
        private EcsWorld _world;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var dragon in _dragons)  
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    ref var triggered = ref _dragons.Get2(dragon);

                    if (triggered.Other.IsAlive() && triggered.Other.Has<Components.DragonBody>())
                    {
                        if (_dragons.GetEntity(dragon) == triggered.Other.Get<Components.DragonBody>().Head)
                        {
                            continue;
                        }
                    }
                
                    if (triggered.Other.IsAlive() && triggered.Other.Has<Obstacles.Components.Obstacle>())
                    {
                        ref var dragonHead = ref _dragons.Get1(dragon);

                        var foodPrefab = controller.LevelsConfigs.GoodsConfig.FoodPrefab;
                 
                        for (int i = 0; i <  dragonHead.BodyParts.Count; i++)
                        {
                            var food = Object.Instantiate(foodPrefab, dragonHead.BodyParts[i].position, Quaternion.identity);
                            food.Spawn(_world.NewEntity(), _world);
                            controller.GoodsPositions.Insert(0, food.transform);
                        }

                        foreach (var bodyParts in dragonHead.BodyParts)
                        {
                            if (bodyParts.TryGetComponent(out EntityRef entityRef))
                            {
                                entityRef.Entity.Destroy();
                            }
                        }
                    }
                }
            }
        }
    }
}