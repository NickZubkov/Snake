using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<ViewHub.UnityView, Components.DragonHead> _dragon;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;

        private EcsWorld _world;

        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;
            
            foreach (var levelData in _levelData)
            {
                foreach (var idx in _dragon)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    
                    ref var dragonHead = ref _dragon.Get2(idx);
                    Collider[] hitColliders = new Collider[levelRunTimeData.MaxGoodsSerchingCount];
                    var spherePosition = dragonHead.HeadTransform.position.Where(y: 0f);
                    var radius = levelRunTimeData.GoodsCollectingRadius * dragonHead.HeadTransform.localScale.x / 2f;
                    Physics.OverlapSphereNonAlloc(spherePosition, radius, hitColliders, levelRunTimeData.GoodsLayerMask);
                    
                    foreach (var collider in hitColliders)
                    {
                        if (collider != null && collider.gameObject.TryGetComponentInParent(out ViewHub.EntityRef entityRef))
                        {
                            if (entityRef.Entity.Has<Goods.Components.Food>())
                            {
                                ref var bodySpawningSignal = ref _dragon.GetEntity(idx).Get<LevelController.Components.DragonBodySpawningSignal>();
                                bodySpawningSignal.DragonHead = dragonHead;
                                bodySpawningSignal.BodyPrefab = dragonHead.DragonConfig.BodyPrefab;
                                dragonHead.Points += dragonHead.PointBonusMultiplyer;
                                entityRef.Entity.Get<Utils.DestroyTag>();
                            }

                            else if (entityRef.Entity.Has<Goods.Components.Bonus>())
                            {
                                entityRef.Entity.Get<Goods.Components.Bonus>().BonusApplyer.Activate(ref dragonHead);
                                entityRef.Entity.Get<Utils.DestroyTag>();
                            }
                        }
                    }
                } 
            }
        }
    }
}