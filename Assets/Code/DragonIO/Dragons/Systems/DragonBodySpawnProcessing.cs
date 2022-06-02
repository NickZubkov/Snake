using System.Linq;
using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonBodySpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<Components.DragonHeadSpawnedSignal> _headSpawnedSignal;
        private EcsFilter<LevelController.Components.DragonBodySpawningSignal> _bodySpawningSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;

        private EcsWorld _world;
        public void Run()
        {
            if (_headSpawnedSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    var spawnDecrease = _levelData.Get1(levelData).BodyPartSpawnDecrease;
                    foreach (var bodySpawningSignal in _bodySpawningSignal)
                    {
                        ref var spawnSignal = ref _bodySpawningSignal.Get1(bodySpawningSignal);
                        if (spawnSignal.DragonHead.Points % spawnDecrease == 0)
                        {
                            var index = spawnSignal.DragonHead.BodyParts.Count - 1;
                            var bodyPart = Object.Instantiate(spawnSignal.BodyPrefab, spawnSignal.DragonHead.BodyParts[index].position, Quaternion.identity);
                            bodyPart.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                            spawnSignal.DragonHead.BodyParts.Insert(index, bodyPart.transform);
                            var bodyEntity = _world.NewEntity();
                            ref var body = ref bodyEntity.Get<Components.DragonBody>();
                            body.HeadID = spawnSignal.DragonHead.HeadID;
                            body.Head = spawnSignal.DragonHead;
                            bodyPart.Spawn(bodyEntity, _world);
                            spawnSignal.DragonHead.Body.Insert(0, body);
                        }
                    } 
                }
            }
            
            foreach (var headSpawnedSignal in _headSpawnedSignal)
            {
                ref var spawnSignal = ref _headSpawnedSignal.Get1(headSpawnedSignal);
                var legs = Object.Instantiate(spawnSignal.DragonHead.DragonConfig.LegsPrefab, spawnSignal.DragonHead.HeadTransform.position, Quaternion.identity);
                legs.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                spawnSignal.DragonHead.BodyParts.Insert(1, legs.transform);
                var legsEntity = _world.NewEntity();
                ref var legsBody = ref legsEntity.Get<Components.DragonBody>();
                legsBody.HeadID = spawnSignal.DragonHead.HeadID;
                legsBody.Head = spawnSignal.DragonHead;
                
                legs.Spawn(legsEntity, _world);
                spawnSignal.DragonHead.Body.Insert(0, legsBody);

                var tail = Object.Instantiate(spawnSignal.DragonHead.DragonConfig.TailPrefab, spawnSignal.DragonHead.HeadTransform.position, Quaternion.identity);
                tail.transform.parent = spawnSignal.DragonHead.HeadTransform.parent;
                spawnSignal.DragonHead.BodyParts.Insert(2, tail.transform);
                var tailEntity = _world.NewEntity();
                ref var tailbody = ref tailEntity.Get<Components.DragonBody>();
                tailbody.HeadID = spawnSignal.DragonHead.HeadID;
                tailbody.Head = spawnSignal.DragonHead;
                
                tail.Spawn(tailEntity, _world);
                spawnSignal.DragonHead.Body.Insert(0, tailbody);
            }
        }
    }
}