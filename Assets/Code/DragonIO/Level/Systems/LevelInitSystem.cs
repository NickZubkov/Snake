using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Level.Systems
{
    public class LevelInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState, EventGroup.StateEnter> _gamePlay; 
        private EcsFilter<ViewHub.UnityView, Components.Ground> _ground;

        private Data.GameConfig _config;
        private EcsWorld _ecsWorld;
        public void Run()
        {
            if(_gamePlay.IsEmpty())
                return;
            
            var levelConfig = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).LevelConfig;
            
            foreach (var idx in _ground)
            {
                var groundTransform = _ground.Get1(idx).Transform;
                groundTransform.localScale = new Vector3(levelConfig.LevelSize, groundTransform.localScale.y, levelConfig.LevelSize);
            }
            
            var wallSize = levelConfig.LevelSize * Mathf.Sin(180 * Mathf.Deg2Rad / levelConfig.WallsCount);
            var placeRadius = wallSize / (2 * Mathf.Tan(180 * Mathf.Deg2Rad / levelConfig.WallsCount));
            
            for(int i = 0; i < levelConfig.WallsCount; i++)
            {
                var angle = 360 / levelConfig.WallsCount * Mathf.Deg2Rad * i;
                var position = new Vector3(Mathf.Cos(angle) * placeRadius, 0, Mathf.Sin(angle) * placeRadius);
                var wall = Object.Instantiate(levelConfig.WallPrefab, position, Quaternion.identity);
                wall.transform.LookAt(Vector3.zero);
                wall.transform.localScale = new Vector3(wallSize, wall.transform.localScale.y, wall.transform.localScale.z);
                wall.Spawn(_ecsWorld.NewEntity(), _ecsWorld);
            }
        }
    }
}