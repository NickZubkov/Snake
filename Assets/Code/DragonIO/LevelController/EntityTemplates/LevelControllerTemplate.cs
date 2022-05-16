using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.LevelController.EntityTemplates
{
    public class LevelControllerTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.LevelController>();
            entity.Get<Components.LevelControllerSpawnedSignal>();
        }
    }
}