using Leopotam.Ecs;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerInitSystem : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead> _player;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
            
        private EcsWorld _world;

        public void Run()
        {
            if(_player.IsEmpty())
                return;
            
            foreach (var player in _player)
            {
                foreach (var levelData in _levelData)
                {
                    
                }
                
            }
        }
    }
}