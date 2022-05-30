using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class LocationWallsSpawnProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<Components.Wall> _walls;
        private EcsFilter<LevelController.Components.WallSpawningSignal> _wallsSpawningSignal;
        
        private Data.GameConfig _config;
        private EcsWorld _ecsWorld;
        public void Run()
        {
            if (!_wallsSpawningSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var data = ref _levelData.Get1(levelData);
                    ref var configs = ref _levelData.Get2(levelData);
                    var angle = (360f / Data.GameConstants.WALLS_COUNT) * Mathf.Deg2Rad * _walls.GetEntitiesCount();
                    var angleOffset = Data.GameConstants.WALLS_ANGLE_OFFSET;
                    var position = new Vector3(Mathf.Cos(angle + angleOffset) * data.ObjectsMaxSpawnRadius, 0, Mathf.Sin(angle + angleOffset) * data.ObjectsMaxSpawnRadius);
                    var wall = Object.Instantiate(configs.LocationConfig.WallPrefab, position, Quaternion.identity);
                    wall.transform.LookAt(Vector3.zero);
                    wall.transform.localScale = new Vector3(data.WallSize, wall.transform.localScale.y, wall.transform.localScale.z);
                    wall.Spawn(_ecsWorld.NewEntity(), _ecsWorld);
                }
            }
        }
    }
}