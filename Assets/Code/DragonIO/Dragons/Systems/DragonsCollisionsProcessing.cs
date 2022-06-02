using DG.Tweening;
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
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
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
                                ReleaseCollision(currentLevelConfigs, ref body.Head);
                            }
                            else if (!dragonHead.IsShieldActive)
                            {
                                levelDataEntity.Get<Goods.Components.PlayDeathVFXSignal>().PlayPosition = dragonHead.HeadTransform.position;
                                ReleaseCollision(currentLevelConfigs, ref dragonHead);
                            }
                            
                        }
                        
                        else if (triggered.Other.Has<Location.Components.Obstacle>())
                        {
                            if (triggered.Other.Get<Location.Components.Obstacle>().DestroyThreshold < dragonHead.BodyParts.Count)
                            {
                                var transform = triggered.Collider.transform;
                                var sequence = DOTween.Sequence();
                                sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 0, 0));
                                sequence.Append(transform.DOScale(Vector3.one * 0.05f, 0.2f));
                                triggered.Other.Del<Location.Components.Obstacle>();
                                triggered.Other.Get<Utils.DestroyTag>().DestroyTime = 0.5f;
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
                            ReleaseCollision(currentLevelConfigs, ref dragonHead);
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
                var position = dragonHead.BodyParts[i].position.Where(y: 0f);
                var food = Object.Instantiate(foodPrefab, position, Quaternion.identity);
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