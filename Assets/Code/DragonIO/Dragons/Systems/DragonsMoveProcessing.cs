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
        private Vector3 _targetBodyPoint;

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
                foreach (var bodyPart in dragon.BodyParts)
                {
                    _targetBodyPoint = dragon.PositionsHistory[Mathf.Clamp(index * dragon.DragonConfig.Gap, 0, dragon.PositionsHistory.Count - 1)];
                    _bodyMoveDirection = _targetBodyPoint - bodyPart.position;
                    bodyPart.Translate(_bodyMoveDirection * _time.DeltaTime * dragon.DragonConfig.Speed, Space.World);
                    bodyPart.LookAt(_targetBodyPoint);
                    index++;
                }
            }
        }
    }
}