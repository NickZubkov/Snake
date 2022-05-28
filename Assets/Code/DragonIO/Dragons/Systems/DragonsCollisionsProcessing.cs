using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollisionsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.DragonHead, UPhysics.Triggered> _dragons;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        
        private EcsWorld _world;
        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var dragon in _dragons)  
            {
                foreach (var levelData in _levelData)
                {
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    ref var triggered = ref _dragons.Get2(dragon);
                    ref var dragonHead = ref _dragons.Get1(dragon);

                    if (triggered.Other.IsAlive() && triggered.Other.Has<Components.DragonBody>())
                    {
                        if (dragonHead.HeadID == triggered.Other.Get<Components.DragonBody>().HeadID)
                        {
                            continue;
                        }
                    }
                
                    if (triggered.Other.IsAlive() && triggered.Other.Has<Location.Components.Obstacle>())
                    {
                        if (dragonHead.IsShieldActive)
                        {
                            continue;
                        }

                        var foodPrefab = currentLevelConfigs.GoodsConfig.FoodPrefab;
                 
                        for (int i = 0; i <  dragonHead.BodyParts.Count; i++)
                        {
                            var food = Object.Instantiate(foodPrefab, dragonHead.BodyParts[i].position, Quaternion.identity);
                            food.Spawn(_world.NewEntity(), _world);
                        }

                        foreach (var bodyParts in dragonHead.BodyParts)
                        {
                            if (bodyParts.TryGetComponent(out EntityRef entityRef))
                            {
                                entityRef.Entity.Get<Utils.DestroyTag>();
                            }
                        }
                    }
                }
            }
        }
    }
}