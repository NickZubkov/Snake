using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class LocationInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState, EventGroup.StateEnter> _gamePlay;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private EcsFilter<ViewHub.UnityView, Components.Ground> _ground;

        private Data.GameConfig _config;
        private EcsWorld _ecsWorld;
        public void Run()
        {
            if(_gamePlay.IsEmpty())
                return;
            
            foreach (var levelController in _levelController)
            {
                ref var controller = ref _levelController.Get1(levelController);
                
                foreach (var idx in _ground)
                {
                    var groundTransform = _ground.Get1(idx).Transform;
                    groundTransform.localScale = new Vector3(controller.LevelsConfigs.LocationConfig.LevelSize, groundTransform.localScale.y, controller.LevelsConfigs.LocationConfig.LevelSize);
                }
            
                for(int i = 0; i < controller.LevelsConfigs.LocationConfig.WallsCount; i++)
                {
                    var angle = (360 / controller.LevelsConfigs.LocationConfig.WallsCount) * Mathf.Deg2Rad * i;
                    var angleOffset = 5 * Mathf.Deg2Rad;
                    var position = new Vector3(Mathf.Cos(angle + angleOffset) * controller.PlaceRadius, 0, Mathf.Sin(angle + angleOffset) * controller.PlaceRadius);
                    var wall = Object.Instantiate(controller.LevelsConfigs.LocationConfig.WallPrefab, position, Quaternion.identity);
                    wall.transform.LookAt(Vector3.zero);
                    wall.transform.localScale = new Vector3(controller.WallSize, wall.transform.localScale.y, wall.transform.localScale.z);
                    wall.Spawn(_ecsWorld.NewEntity(), _ecsWorld);
                }
            }
        }
    }
}