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
            foreach (var signal in _scalngSignal)
            {
                if (_scalngSignal.Get1(signal).DragonHead.HeadID == -1)
                {
                    foreach (var levelData in _levelData)
                    {
                        ref var levelRunTimeData = ref _levelData.Get1(levelData);
                        levelRunTimeData.CinemachineTransposer.m_FollowOffset += new Vector3(0, levelRunTimeData.CameraOffset, 0);
                    }
                }
            }
        }
    }
}