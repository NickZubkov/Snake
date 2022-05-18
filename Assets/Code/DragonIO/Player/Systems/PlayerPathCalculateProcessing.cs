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
                ref var player = ref _player.Get3(idx);
                // Calc head position
                player.TargetHeadPoint += (_player.Get1(idx).Transform.forward + _headMoveDirection) * _time.DeltaTime * dragonHead.Config.Speed;
                

                // Store position history
                dragonHead.PositionsHistory.Insert(0, player.TargetHeadPoint);

                // Clear position history
                int targetCount = dragonHead.BodyParts.Count * dragonHead.Config.Gap;
                if (dragonHead.PositionsHistory.Count > targetCount)
                {
                    var countToRemove = dragonHead.PositionsHistory.Count - targetCount;
                    dragonHead.PositionsHistory.RemoveRange(targetCount, countToRemove);
                }
            }
        }
    }
}