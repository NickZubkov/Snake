using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Obstacles.EntityTemplates
{
    public class ObstacleTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Obstacle>();
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}