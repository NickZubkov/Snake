using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using MoreMountains.NiceVibrations;
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
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var triggered = ref _dragons.Get2(dragon);
                    ref var dragonHead = ref _dragons.Get1(dragon);
                    ref var levelDataEntity = ref _levelData.GetEntity(levelData);

                    if (triggered.Other.IsAlive())
                    {
                        if (triggered.Other.Has<Components.DragonBody>())
                        {
                            ref var body = ref triggered.Other.Get<Components.DragonBody>();
                            if (dragonHead.HeadID == body.HeadID)
                            {
                                continue;
                            }

                            if (body.HeadID == -1 && dragonHead.BodyParts.Count >= body.Head.BodyParts.Count)
                            {
                                Misc.PlayVibro(HapticTypes.SoftImpact);
                                _world.NewEntity().Get<LevelController.Components.ChangeCameraFollowSignal>().FollowTransform = dragonHead.HeadTransform;
                            }

                            if (dragonHead.BodyParts.Count >= body.Head.BodyParts.Count)
                            {
                                if (!body.Head.IsShieldActive)
                                {
                                    if (dragonHead.HeadID == -1)
                                    {
                                        _world.NewEntity().Get<UI.Components.FlyingTextSignal>();
                                    }
                                    ReleaseCollision(ref levelRunTimeData, ref body.Head);
                                }
                            }
                            else if (!dragonHead.IsShieldActive)
                            {
                                levelDataEntity.Get<Goods.Components.PlayDeathVFXSignal>().PlayPosition = dragonHead.HeadTransform.position;
                                ReleaseCollision(ref levelRunTimeData, ref dragonHead);
                            }
                            
                        }
                        
                        else if (triggered.Other.Has<Location.Components.Obstacle>())
                        {
                            if (triggered.Other.Get<Location.Components.Obstacle>().DestroyThreshold < dragonHead.BodyParts.Count)
                            {
                                triggered.Other.Get<Location.Components.ObstaclePlayVFXSignal>();
                                continue;
                            }
                            if (dragonHead.IsShieldActive)
                            {
                                continue;
                            }

                            if (_dragons.GetEntity(dragon).Has<Player.Components.Player>())
                            {
                                Misc.PlayVibro(HapticTypes.SoftImpact);
                            }
                            
                            levelDataEntity.Get<Goods.Components.PlayDeathVFXSignal>().PlayPosition = dragonHead.HeadTransform.position;
                            ReleaseCollision(ref levelRunTimeData, ref dragonHead);
                        }
                        else if (triggered.Other.Has<Location.Components.Wall>())
                        {
                            if (dragonHead.IsShieldActive)
                            {
                                dragonHead.TargetHeadDirection = (Vector3.zero - dragonHead.HeadTransform.position).normalized;
                                if (!dragonHead.LockDirection)
                                {
                                    dragonHead.LockDirectionTimer = 2f;
                                    dragonHead.LockDirection = true;
                                }
                                continue;
                            }
                            
                            if (_dragons.GetEntity(dragon).Has<Player.Components.Player>())
                            {
                                Misc.PlayVibro(HapticTypes.SoftImpact);
                            }
                            levelDataEntity.Get<Goods.Components.PlayDeathVFXSignal>().PlayPosition = dragonHead.HeadTransform.position;
                            ReleaseCollision(ref levelRunTimeData, ref dragonHead);
                        }
                    }
                }
            }
        }

        private void ReleaseCollision(ref LevelController.Components.LevelRunTimeData levelRunTimeData, ref Components.DragonHead dragonHead)
        {
            var positions = new Queue<Vector3>(); 
            foreach (var bodyParts in dragonHead.BodyParts)
            {
                if (bodyParts.TryGetComponent(out EntityRef entityRef))
                {
                    entityRef.Entity.Get<Utils.DestroyTag>();
                }
                
                positions.Enqueue(bodyParts.position);
            }
            levelRunTimeData.FoodSpawningPositions.Enqueue(positions);
        }
    }
}