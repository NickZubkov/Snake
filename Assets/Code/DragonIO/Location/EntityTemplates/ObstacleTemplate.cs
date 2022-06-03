using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class ObstacleTemplate : ViewElement
    {
        [SerializeField] private int _destroyThreshold;
        [SerializeField] private ParticleSystem _puffVFX;
        [SerializeField] private Transform _view;
        [SerializeField] private List<MeshRenderer> _viewRenderers;
        
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            ref var obstacle = ref entity.Get<Components.Obstacle>();
            obstacle.DestroyThreshold = _destroyThreshold;
            obstacle.PuffVFX = _puffVFX;
            obstacle.ViewMeshRenderers = _viewRenderers;
            obstacle.View = _view;
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}