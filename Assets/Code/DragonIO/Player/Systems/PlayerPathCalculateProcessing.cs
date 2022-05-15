using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;

        private EcsFilter<Joystick.JoystickData> _joystick;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.PlayerTag> _player;

        private Utils.TimeService _time;

        // runtime data
        private Vector3 _headMoveDirection;
        private Vector3 _targetHeadPoint;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var idx in _joystick)
            {
                var joystickInput = _joystick.Get1(idx).Input;
                _headMoveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y);
            }

            if (_headMoveDirection.sqrMagnitude <= 0f)
                return;

            foreach (var idx in _player)
            {
                ref var dragon = ref _player.Get2(idx);
                
                // Calc head position
                _targetHeadPoint += _headMoveDirection * _time.DeltaTime * dragon.Config.Speed;

                // Store position history
                dragon.PositionsHistory.Insert(0, _targetHeadPoint);

                // Clear position history
                int targetCount = dragon.BodyParts.Count * dragon.Config.Gap;
                if (dragon.PositionsHistory.Count > targetCount)
                {
                    var countToRemove = dragon.PositionsHistory.Count - targetCount;
                    dragon.PositionsHistory.RemoveRange(targetCount, countToRemove);
                }
            }
        }
    }
}