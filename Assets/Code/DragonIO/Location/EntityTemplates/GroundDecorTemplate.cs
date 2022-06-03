using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class GroundDecorTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.GroundDecor>();
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}