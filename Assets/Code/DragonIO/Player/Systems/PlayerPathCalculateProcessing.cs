using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;

        private EcsFilter<Joystick.JoystickData> _joystick;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Player> _player;

        private Utils.TimeService _time;

        // runtime data
        private Vector3 _headMoveDirection;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var idx in _joystick)
            {
                var joystickInput = _joystick.Get1(idx).Input;
                _headMoveDirection = new Vector3(joystickInput.x, 0f, joystickInput.y);
            }
            
            foreach (var idx in _player)
            {
                ref var dragonHead = ref _player.Get2(idx);
                dragonHead.TargetHeadDirection = _headMoveDirection;
            }
        }
    }
}