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
        private Data.GameConfig _config;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var dragon in _dragons)  
            {
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

                    var foodPrefab = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).GoodsConfig.FoodPrefab;
                 
                    for (int i = 0; i <  dragonHead.BodyParts.Count; i++)
                    {
                        var food = Object.Instantiate(foodPrefab, dragonHead.BodyParts[i].position, Quaternion.identity);
                        food.Spawn(_world.NewEntity(), _world);
                        
                        foreach (var controller in _levelController)
                        {
                            _levelController.Get1(controller).GoodsPositions.Insert(0, food.transform);
                        }
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