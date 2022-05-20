using Leopotam.Ecs;

namespace Modules.DragonIO.UI.Systems
{
    public class UIUpdateProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.LeaderBoardView> _leaderBoard;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead> _dragons;
        public void Run()
        {
            foreach (var leaderBoard in _leaderBoard)
            {
                int points = 0;
                string name = "";
                foreach (var dragon in _dragons)       
                {
                    ref var dragonHead = ref _dragons.Get2(dragon);
                    if (dragonHead.Points > points)
                    {
                        points = dragonHead.Points;
                        name = _dragons.Get1(dragon).GameObject.name;
                    }
                }
                
                _leaderBoard.Get1(leaderBoard).View.SetBoardValue(name, points);
            }
        }
    }
}