using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsMoveProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<ViewHub.UnityView, Components.DragonHead> _dragons;
        private Utils.TimeService _time;

        // runtime data
        private Vector3 _bodyMoveDirection;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _dragons)
            {
                ref var dragon = ref _dragons.Get2(idx);
                ref var dragonHeadTransform = ref _dragons.Get1(idx).Transform;

                int index = 0;
                var gap = 10f / (dragon.DragonConfig.Gap + dragonHeadTransform.localScale.x - 1);
                for (int i = 0; i < dragon.BodyParts.Count; i++)
                {
                    if (i == 0)
                    {
                        if (dragon.TargetHeadDirection != Vector3.zero)
                        {
                            float step = dragon.RotationSpeed * 100f * _time.DeltaTime / dragonHeadTransform.localScale.x;
                            Quaternion lookRotation = Quaternion.LookRotation(dragon.TargetHeadDirection);
                            var tmp = Quaternion.RotateTowards(dragon.BodyParts[i].rotation, lookRotation, step);
                            dragon.BodyParts[i].rotation = Quaternion.RotateTowards(dragon.BodyParts[i].rotation, tmp, step);
                        }
                        dragon.BodyParts[i].Translate(dragon.BodyParts[i].forward * _time.DeltaTime * dragon.MovementSpeed, Space.World);
                    }
                    else
                    {
                        _bodyMoveDirection = dragon.BodyParts[i - 1].position - dragon.BodyParts[i].position;
                        dragon.BodyParts[i].Translate(_bodyMoveDirection * _time.DeltaTime * dragon.MovementSpeed * gap, Space.World);
                        dragon.BodyParts[i].LookAt(dragon.BodyParts[i - 1].position);
                    }
                    index++;
                }
            }
        }
    }
}