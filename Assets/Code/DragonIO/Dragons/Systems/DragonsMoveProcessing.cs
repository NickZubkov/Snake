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
                
                if (dragon.PositionsHistory.Count == 0)
                    continue;
                
                int index = 0;
                for (int i = 0; i < dragon.BodyParts.Count; i++)
                {
                    if (i == 0)
                    {
                        if (dragon.PositionsHistory[0] != Vector3.zero)
                        {
                            var cross = Vector3.Cross(dragon.BodyParts[i].forward, dragon.PositionsHistory[0]);

                            if (cross.y >= dragon.DragonConfig.Speed * 0.01f)
                            {
                                dragon.BodyParts[i].Rotate(Vector3.up * dragon.DragonConfig.Speed/2);
                            }
                            else if (cross.y < dragon.DragonConfig.Speed * -0.01f)
                            {
                                dragon.BodyParts[i].Rotate(Vector3.down * dragon.DragonConfig.Speed/2);
                            }
                        }
                        dragon.BodyParts[i].Translate(dragon.BodyParts[i].forward * _time.DeltaTime * dragon.DragonConfig.Speed, Space.World);
                    }
                    else
                    {
                        _bodyMoveDirection = dragon.BodyParts[i - 1].position - dragon.BodyParts[i].position;
                        var gap = 10f / dragon.DragonConfig.Gap;
                        dragon.BodyParts[i].Translate(_bodyMoveDirection * _time.DeltaTime * dragon.DragonConfig.Speed * gap, Space.World);
                        dragon.BodyParts[i].LookAt(dragon.BodyParts[i - 1].position);
                    }
                    index++;
                }
            }
        }
    }
}