using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Location.ViewComponents
{
    public class ObstacleViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Obstacle>();
        }
    }
}