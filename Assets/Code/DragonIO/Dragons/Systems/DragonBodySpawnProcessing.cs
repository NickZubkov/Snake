using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonBodySpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.DragonHeadSpawnedSignal> _headSpawnedSignal;
        private EcsFilter<LevelController.Components.DragonBodySpawningSignal> _bodySpawningSignal;

        private EcsWorld _world;
        public void Run()
        {
            foreach (var bodySpawningSignal in _bodySpawningSignal)
            {
                ref var spawnSignal = ref _bodySpawningSignal.Get1(bodySpawningSignal);
                var index = spawnSignal.DragonHead.BodyParts.Count - 1;
                var bodyPart = Object.Instantiate(spawnSignal.BodyPrefab, spawnSignal.DragonHead.BodyParts[index].position, Quaternion.identity);
                bodyPart.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                spawnSignal.DragonHead.BodyParts.Insert(index, bodyPart.transform);
                var bodyEntity = _world.NewEntity();
                bodyEntity.Get<Components.DragonBody>().HeadID = spawnSignal.DragonHead.HeadID;
                bodyPart.Spawn(bodyEntity, _world);
            }

            foreach (var headSpawnedSignal in _headSpawnedSignal)
            {
                ref var spawnSignal = ref _headSpawnedSignal.Get1(headSpawnedSignal);
                var legs = Object.Instantiate(spawnSignal.DragonHead.DragonConfig.LegsPrefab, spawnSignal.DragonHead.HeadTransform.position, Quaternion.identity);
                legs.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                spawnSignal.DragonHead.BodyParts.Insert(1, legs.transform);
                var legsEntity = _world.NewEntity();
                legsEntity.Get<Components.DragonBody>().HeadID = spawnSignal.DragonHead.HeadID;
                legs.Spawn(legsEntity, _world);

                var tail = Object.Instantiate(spawnSignal.DragonHead.DragonConfig.TailPrefab, spawnSignal.DragonHead.HeadTransform.position, Quaternion.identity);
                tail.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                spawnSignal.DragonHead.BodyParts.Insert(2, tail.transform);
                var tailEntity = _world.NewEntity();
                tailEntity.Get<Components.DragonBody>().HeadID = spawnSignal.DragonHead.HeadID;
                tail.Spawn(tailEntity, _world);
            }
        }
    }
}