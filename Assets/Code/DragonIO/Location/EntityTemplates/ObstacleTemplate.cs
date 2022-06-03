using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class ObstacleTemplate : ViewElement
    {
        [SerializeField] private int _destroyThreshold;
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Obstacle>().DestroyThreshold = _destroyThreshold;
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}