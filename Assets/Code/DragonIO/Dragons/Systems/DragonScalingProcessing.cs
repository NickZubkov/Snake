using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonScalingProcessing : IEcsRunSystem
    {
        private EcsFilter<LevelController.Components.DragonBodySpawningSignal> _scalngSignal;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        public void Run()
        {
            foreach (var scalingSignal in _scalngSignal)
            {
                foreach (var levelData in _levelData)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    ref var dragonHead = ref _scalngSignal.Get1(scalingSignal).DragonHead;
                    var newScale = dragonHead.HeadTransform.localScale + (Vector3.one * levelRunTimeData.DragonScalingFactor);
                    foreach (var part in dragonHead.BodyParts)
                    {
                        part.localScale = newScale;
                        var newPosition = new Vector3(part.localPosition.x, part.localScale.y / 2f, part.localPosition.z);
                        part.localPosition = newPosition;
                    }
                }
            }
        }
    }
}