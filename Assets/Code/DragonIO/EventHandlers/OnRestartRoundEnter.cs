using UnityEngine;
using Leopotam.Ecs;
using Modules.EventGroup;

namespace Modules.DragonIO.EventHandlers
{
    /// <summary>
    /// process restart of a level
    /// </summary>
    public class OnRestartRoundEnter : IEcsRunSystem
    {
        readonly EcsFilter<RestartRoundState, StateEnter> _signal;
        private EcsFilter<Player.Components.Player> _player;
        private EcsFilter<Goods.Components.Food> _food;
        private EcsFilter<Goods.Components.Bonus> _bonus;
        private EcsFilter<Dragons.Components.DragonBody> _dragonBody;
        private EcsFilter<Dragons.Components.DragonHead> _dragonHead;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        readonly EcsWorld _world;

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

            // in general enough to just trigger next level state
            EventGroup.StateFactory.CreateState<NextLevelState>(_world);
            
            foreach (var i in _signal)
            {
                // restart is quite tricky process
                // if there are systems with EcsFilter<NextLevelState, EventGroup.StateEnter> before OnRestartRoundEnter 
                // OnRestartRoundEnter system should be moved before them
                // in case of problems with restart processing, try to just reload current scene

                // если есть системы, обрабатывающие EcsFilter<NextLevelState, EventGroup.StateEnter> в основной логике
                // то система рестарта должна идти перед ними для корректной отработки события входа в NextLevelState у этих систем
                // если будут проблемы с обработкой рестарта, то стоит попробовать просто перезагружать сцену

                // force end restart state 
                _signal.GetEntity(i).Destroy();
            }
        }
    }
}