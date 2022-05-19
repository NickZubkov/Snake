using Leopotam.Ecs;

namespace Modules.DragonIO
{
    public class RoundFailedTracker : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Player.Components.Player> _player;
        
        private EcsWorld _world;
        private Data.GameConfig _config;
        public void Run()
        {
            if(_gameplay.IsEmpty())
                return;

            if (_player.IsEmpty())
            {
                EventGroup.StateFactory.CreateState<EventGroup.RoundFailedState>(_world);
            }
        }
    }
}