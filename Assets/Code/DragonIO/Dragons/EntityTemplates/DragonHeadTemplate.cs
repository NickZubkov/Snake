using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Dragons.EntityTemplates
{
    public class DragonHeadTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.DragonHead>() = new Components.DragonHead
            {
                BodyParts = new List<Transform>
                {
                    transform
                },
                PositionsHistory = new List<Vector3>(),
                Points = 0
            };
        }
    }
}