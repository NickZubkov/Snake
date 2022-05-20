using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Goods.ViewComponents
{
    public class BonusViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Bonus>();
            ecsEntity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}