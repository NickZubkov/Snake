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
            ref var dragonHead = ref entity.Get<Components.DragonHead>();
            dragonHead.RotationSpeed = dragonHead.DragonConfig.RotationSpeed;
            dragonHead.MovementSpeed = dragonHead.DragonConfig.MovementSpeed;
            dragonHead.Gap = dragonHead.DragonConfig.Gap;
            dragonHead.DefaultBonusMultiplyer = 1;
            dragonHead.PointBonusMultiplyer = 1;
            dragonHead.DragonName = transform.parent.name;
            dragonHead.HeadTransform = transform;
            dragonHead.BodyParts = new List<Transform>{transform};
            dragonHead.StartBodyCount = dragonHead.DragonConfig.BodySegmentsCount;
            dragonHead.TargetHeadDirection = Vector3.zero;
            dragonHead.Points = 0;
            entity.Get<LevelSpawner.LevelEntityTag>();
        }
    }
}