using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<ViewHub.UnityView, Components.DragonHead, UPhysics.Triggered> _dragon;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;

        private EcsWorld _world;

        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;

            foreach (var idx in _dragon)
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var triggered = ref _dragon.Get3(idx);
                    ref var dragonHead = ref _dragon.Get2(idx);
                    ref var dragonTransform = ref _dragon.Get1(idx);
                    ref var dragonEntity = ref _dragon.GetEntity(idx);
                    if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Food>())
                    {
                        ref var bodySpawningSignal = ref _dragon.GetEntity(idx).Get<LevelController.Components.DragonBodySpawningSignal>();
                        bodySpawningSignal.DragonHead = dragonHead;
                        bodySpawningSignal.BodyPrefab = dragonHead.DragonConfig.BodyPrefab;
                        dragonHead.Points += dragonHead.PointBonusMultiplyer;

                        var newScale = dragonTransform.Transform.localScale + (Vector3.one * levelRunTimeData.DragonScalingFactor);
                        foreach (var part in dragonHead.BodyParts)
                        {
                            part.localScale = newScale;
                            var newPosition = new Vector3(part.position.x, part.position.y + levelRunTimeData.DragonScalingFactor, part.position.z);
                            part.position = newPosition;
                        }

                        if (dragonEntity.Has<Player.Components.Player>())
                        {
                            dragonEntity.Get<LevelController.Components.ChangeCameraOffsetSignal>();
                        }

                        triggered.Other.Get<Utils.DestroyTag>();
                    }
                    else if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Bonus>())
                    {
                        triggered.Other.Get<Goods.Components.Bonus>().BonusApplyer.Activate(ref dragonHead);
                        triggered.Other.Get<Utils.DestroyTag>();
                    }
                }
            }
        }
    }
}