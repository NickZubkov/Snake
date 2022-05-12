using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerMoveProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;

        private EcsFilter<Joystick.JoystickData> _joystick;
        private EcsFilter<Components.Player, ViewHub.UnityView> _player;

        private Utils.TimeService _time;

        // runtime data
        private Vector3 _moveDirection;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var idx in _joystick)
            {
                var joystickInput = _joystick.Get1(idx).Input;
                _moveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y);
            }

            if (_moveDirection.sqrMagnitude <= 0f)
                return;

            foreach (var idx in _player)
            {
                var playerTransform = _player.Get2(idx).Transform;

                // position
                playerTransform.Translate(_moveDirection * _time.DeltaTime * _player.Get1(idx).Config.Speed, Space.World);

                // rotation
                playerTransform.rotation = Quaternion.LookRotation(_moveDirection);
            }
        }
    }
}