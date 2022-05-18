using UnityEngine;
using Leopotam.Ecs;
using Modules.EventGroup;

namespace Modules.DragonIO.EventHandlers
{
    /// <summary>
    /// when player sucessfully passed the level
    /// </summary>
    public class OnRoundCompletedEnter : IEcsRunSystem
    {
        readonly EcsFilter<RoundCompletedState, StateEnter> _signal;
        private EcsFilter<Player.Components.Player> _player;
        private EcsFilter<Goods.Components.Food> _food;
        private EcsFilter<Goods.Components.Bonus> _bonus;
        private EcsFilter<Dragons.Components.DragonBody> _dragonBody;
        private EcsFilter<Dragons.Components.DragonHead> _dragonHead;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        readonly EcsWorld _world;
        readonly Currency.WalletsFacade _wallets;

        public void Run()
        {
            if (_signal.IsEmpty())
                return;
            
            foreach (var idx in _player)
            {
                _player.GetEntity(idx).Destroy();
            }
            
            foreach (var idx in _levelController)
            {
                _levelController.Get1(idx).GoodsPositions.Clear();
            }
            foreach (var idx in _food)
            {
                _food.GetEntity(idx).Destroy();
            }
            foreach (var idx in _bonus)
            {
                _bonus.GetEntity(idx).Destroy();
            }
            foreach (var idx in _dragonHead)
            {
                _dragonHead.GetEntity(idx).Destroy();
            }
            foreach (var idx in _dragonBody)
            {
                _dragonBody.GetEntity(idx).Destroy();
            }

            // show sucess screen
            ref var screen = ref _world.NewEntity().Get<UICoreECS.ShowScreenTag>();
            screen.ID = (int) UI.MainScreens.RoundCompleted;
            screen.Layer = (int) UI.Layers.MainLayer;

            // add reward to player wallet
            _wallets.TransitRewardToPlayer();

            // increase current level
            PlayerLevel.ProgressionInfo.CurrentLevel += 1;
            PlayerLevel.ProgressionInfo.Save();
        }
    }
}