using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Goods.ViewComponents
{
    public class FoodViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Food>();
        }
    }
}