using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Dragons.EntityTemplates
{
    public class DragonBodyTemplate: ViewElement
    {
        [SerializeField] private List<MeshRenderer> _viewRenderers;
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<LevelSpawner.LevelEntityTag>();
            entity.Get<Components.DragonBody>().ViewRenderers = _viewRenderers;
        }
    }
}