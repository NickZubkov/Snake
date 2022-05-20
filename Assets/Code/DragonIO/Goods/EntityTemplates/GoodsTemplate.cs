using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Goods.EntityTemplates
{
    public class GoodsTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}