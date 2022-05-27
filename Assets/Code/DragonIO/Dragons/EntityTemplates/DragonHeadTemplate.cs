using System.Collections.Generic;
using Leopotam.Ecs;
using Modules.ViewHub;
using UnityEngine;

namespace Modules.DragonIO.Dragons.EntityTemplates
{
    public class DragonHeadTemplate : ViewElement
    {
        private EcsEntity _entity;
        public override void OnSpawn(EcsEntity entity, EcsWorld world)
        {
            base.OnSpawn(entity, world);
            entity.Get<Components.DragonHead>() = new Components.DragonHead
            {
                BodyParts = new List<Transform>
                {
                    transform
                },
                TargetHeadDirection = Vector3.zero,
                Points = 0
            };
            entity.Get<LevelSpawner.LevelEntityTag>();
            _entity = entity;
        }

        public void AddEnemyComponent(Data.EnemyConfig enemyConfig)
        {
            ref var entity = ref _entity.Get<Enemy.Components.Enemy>();
            entity.EnemyConfig = enemyConfig;
            entity.ChangeDirectionTimer = 0;
            entity.SerchRadiusThreshold = enemyConfig.SerchRadiusThreshold;
            entity.TimeToChangeDirection = enemyConfig.TimeToChangeDirection;
            entity.LayerMask = 1 << 7;
            _entity.Get<Enemy.Components.EnemyHeadSpawnedSignal>();
            ref var dragonHad = ref _entity.Get<Components.DragonHead>();
            dragonHad.DragonConfig = enemyConfig;
            dragonHad.RotationSpeed = dragonHad.DragonConfig.RotationSpeed;
            dragonHad.MovementSpeed = dragonHad.DragonConfig.MovementSpeed;
            dragonHad.DefaultMultiplyer = 1;
            dragonHad.PointBonusMultiplyer = 1;
            dragonHad.DragonName = transform.parent.name;
            
        }
        public void AddPlayerComponent(Data.PlayerConfig playerConfig)
        {
            ref var dragonHad = ref _entity.Get<Components.DragonHead>();
            dragonHad.DragonConfig = playerConfig;
            dragonHad.RotationSpeed = dragonHad.DragonConfig.RotationSpeed;
            dragonHad.MovementSpeed = dragonHad.DragonConfig.MovementSpeed;
            dragonHad.DefaultMultiplyer = 1;
            dragonHad.PointBonusMultiplyer = 1;
            dragonHad.DragonName = transform.parent.name;
        }
    }
}