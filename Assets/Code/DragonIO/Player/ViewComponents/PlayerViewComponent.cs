using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Player.ViewComponents
{
    public class PlayerViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Player>();
            ecsEntity.Get<LevelSpawner.LevelEntityTag>();
            ecsEntity.Get<Components.PlayerHeadSpawnedSignal>();
        }
    }
}