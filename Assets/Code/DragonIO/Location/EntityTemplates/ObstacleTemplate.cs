using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class ObstacleTemplate : ViewElement
    {
        [SerializeField] private int _destroyThreshold;
        [SerializeField] private List<MeshRenderer> _viewRenderers;
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Obstacle>().DestroyThreshold = _destroyThreshold;
            entity.Get<Components.Obstacle>().ViewMeshRenderers = _viewRenderers;
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}