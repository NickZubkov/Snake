using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Dragons.Systems
{
    public class DragonsMoveProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Components.DragonHead> _dragons;
        private Utils.TimeService _time;

        // runtime data
        private Vector3 _bodyMoveDirection;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _dragons)
            {
                ref var dragon = ref _dragons.Get1(idx);

                int index = 0;
                for (int i = 0; i < dragon.BodyParts.Count; i++)
                {
                    if (i == 0)
                    {
                        if (dragon.TargetHeadDirection != Vector3.zero)
                        {
                            var cross = Vector3.Cross(dragon.BodyParts[i].forward, dragon.TargetHeadDirection);
                            var angle = dragon.RotationSpeed * _time.DeltaTime * dragon.TargetHeadDirection.magnitude;
                            if (cross.y > 0.01f)
                            {
                                dragon.BodyParts[i].Rotate(Vector3.up * angle);
                            }
                            else if (cross.y < -0.01f)
                            {
                                dragon.BodyParts[i].Rotate(Vector3.down * angle);
                            }
                        }
                        dragon.BodyParts[i].Translate(dragon.BodyParts[i].forward * _time.DeltaTime * dragon.DragonConfig.MovementSpeed, Space.World);
                    }
                    else
                    {
                        _bodyMoveDirection = dragon.BodyParts[i - 1].position - dragon.BodyParts[i].position;
                        var gap = 10f / dragon.DragonConfig.Gap;
                        dragon.BodyParts[i].Translate(_bodyMoveDirection * _time.DeltaTime * dragon.DragonConfig.MovementSpeed * gap, Space.World);
                        dragon.BodyParts[i].LookAt(dragon.BodyParts[i - 1].position);
                    }
                    index++;
                }
            }
        }
    }
}