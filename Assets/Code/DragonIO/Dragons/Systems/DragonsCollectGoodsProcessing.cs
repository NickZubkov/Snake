using Leopotam.Ecs;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsCollectGoodsProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gamePlay;
        private EcsFilter<ViewHub.UnityView, Components.DragonHead, UPhysics.Triggered> _dragon;

        private EcsWorld _world;

        public void Run()
        {
            if (_gamePlay.IsEmpty())
                return;

            foreach (var idx in _dragon)
            {
                ref var triggered = ref _dragon.Get3(idx);
                ref var dragonHead = ref _dragon.Get2(idx);
                if (triggered.Other.IsAlive() && triggered.Other.Has<Goods.Components.Food>())
                {
                    ref var bodySpawningSignal = ref _dragon.GetEntity(idx).Get<LevelController.Components.DragonBodySpawningSignal>();
                    bodySpawningSignal.DragonHead = dragonHead;
                    bodySpawningSignal.BodyPrefab = dragonHead.DragonConfig.BodyPrefab;
                    dragonHead.Points += dragonHead.PointBonusMultiplyer;
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