using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Dragons.EntityTemplates
{
    public class DragonBodyTemplate: ViewElement
    {
        private EcsEntity _entity;
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<LevelSpawner.LevelEntityTag>();
            _entity = entity;
        }
    }
}