using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Location.Systems
{
    public class LocationGroundInitSystem : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState, EventGroup.StateEnter> _gamePlay;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private EcsFilter<ViewHub.UnityView, Components.Ground> _ground;

        private Data.GameConfig _config;
        private EcsWorld _ecsWorld;
        public void Run()
        {
            if(_gamePlay.IsEmpty())
                return;
            
            foreach (var levelData in _levelData)
            {
                ref var configs = ref _levelData.Get2(levelData);
                
                foreach (var idx in _ground)
                {
                    var groundTransform = _ground.Get1(idx).Transform;
                    groundTransform.localScale = new Vector3(configs.LocationConfig.LevelSize, groundTransform.localScale.y, configs.LocationConfig.LevelSize);
                }
            }
        }
    }
}