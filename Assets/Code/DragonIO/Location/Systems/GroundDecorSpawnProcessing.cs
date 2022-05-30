using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class GroundDecorSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<LevelController.Components.GroundDecorSpawningSignal> _groundDecorSpawningSignal;

        private EcsWorld _world;
        public void Run()
        {
            if (!_groundDecorSpawningSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    foreach (var player in _player)
                    {
                        ref var levelRunTimeData = ref _levelData.Get1(levelData);
                        ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                        
                        var randomPoint = Random.insideUnitCircle * levelRunTimeData.ObjectsMaxSpawnRadius;
                        var idx = Random.Range(0, currentLevelConfigs.GroundConfig.GroundDecorPrefabs.Count);
                        var prefab = currentLevelConfigs.GroundConfig.GroundDecorPrefabs[idx];
                        var position = new Vector3(randomPoint.x, prefab.transform.position.y, randomPoint.y);

                        var angle = new Vector3(0, Random.Range(0, 180f), 0);
                        var groundDecor = Object.Instantiate(prefab, position, Quaternion.Euler(angle));
                        groundDecor.Spawn(_world.NewEntity(), _world);
                    }
                }
            }
        }
    }
}