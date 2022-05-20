using Leopotam.Ecs;

namespace Modules.DragonIO
{
    public class GameInit : IEcsInitSystem
    {
        readonly EcsWorld _world;

        public void Init()
        {
            // load last saved level info
            PlayerLevel.ProgressionInfo.Load();
            
            // start next level
            EventGroup.StateFactory.CreateState<EventGroup.NextLevelState>(_world);
            
            // spawn gameplay screen
            ref var screen = ref _world.NewEntity().Get<UICoreECS.ShowScreenTag>();
            screen.ID = 0;
            screen.Layer = (int)UI.Layers.MainLayer;
        }
    }
}
