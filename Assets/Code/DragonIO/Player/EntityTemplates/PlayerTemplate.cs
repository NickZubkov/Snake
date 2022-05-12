using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Player.EntityTemplates
{
    public class PlayerTemplate : ViewElement
    {
        
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Player>();
            entity.Get<Components.PlayerSpawnedSignal>();
        }
    }
}