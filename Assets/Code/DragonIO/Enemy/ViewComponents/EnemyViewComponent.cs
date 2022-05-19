using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Enemy.ViewComponents
{
    public class EnemyViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Enemy>();
            ecsEntity.Get<LevelSpawner.LevelEntityTag>();
            ecsEntity.Get<Components.EnemyHeadSpawnedSignal>();
        }
    }
}