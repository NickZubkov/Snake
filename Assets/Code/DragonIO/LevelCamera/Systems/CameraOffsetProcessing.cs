using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelCamera.Systems
{
    public class CameraOffsetProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.DragonBodySpawningSignal> _scalngSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        public void Run()
        {
            if (!_scalngSignal.IsEmpty())
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    levelRunTimeData.CinemachineTransposer.m_FollowOffset += new Vector3(0, levelRunTimeData.DragonScalingFactor, 0);
                }
            }
        }
    }
}