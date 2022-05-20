using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class WallTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Wall>();
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}