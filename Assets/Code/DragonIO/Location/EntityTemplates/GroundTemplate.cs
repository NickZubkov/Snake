﻿using Leopotam.Ecs;
using Modules.ViewHub;

namespace Modules.DragonIO.Location.EntityTemplates
{
    public class GroundTemplate : ViewElement
    {
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.Ground>();
        }
    }
}