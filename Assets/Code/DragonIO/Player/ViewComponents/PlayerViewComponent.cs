using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Player.ViewComponents
{
    public class PlayerViewComponent : ViewComponent
    {
        public override void EntityInit(EcsEntity ecsEntity, EcsWorld ecsWorld, bool parentOnScene)
        {
            ecsEntity.Get<Components.Player>().TargetHeadPoint = Vector3.zero;
            ecsEntity.Get<LevelSpawner.LevelEntityTag>();
            ecsEntity.Get<Components.PlayerHeadSpawnedSignal>();
        }
    }
}