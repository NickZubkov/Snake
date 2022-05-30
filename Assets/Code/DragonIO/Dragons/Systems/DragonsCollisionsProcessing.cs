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

                    if (triggered.Other.IsAlive())
                    {
                        if (triggered.Other.Has<Components.DragonBody>())
                        {
                            if (dragonHead.HeadID == triggered.Other.Get<Components.DragonBody>().HeadID)
                            {
                                continue;
                            }
                            
                            if (dragonHead.IsShieldActive)
                            {
                                continue;
                            }
                            
                            ReleaseCollision(currentLevelConfigs, ref dragonHead);
                        }
                        
                        else if (triggered.Other.Has<Location.Components.Obstacle>() && !triggered.Other.Has<Location.Components.Wall>())
                        {
                            if (triggered.Other.Get<Location.Components.Obstacle>().DestroyThreshold < dragonHead.BodyParts.Count)
                            {
                                triggered.Other.Get<Utils.DestroyTag>();
                                continue;
                            }
                            if (dragonHead.IsShieldActive)
                            {
                                continue;
                            }
                            ReleaseCollision(currentLevelConfigs, ref dragonHead);
                        }
                    }
                }
            }
        }

        private void ReleaseCollision(LevelController.Components.CurrentLevelConfigs currentLevelConfigs, ref Components.DragonHead dragonHead)
        {
            var foodPrefab = currentLevelConfigs.GoodsConfig.FoodPrefab;

            for (int i = 0; i < dragonHead.BodyParts.Count; i++)
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