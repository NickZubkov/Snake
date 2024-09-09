using Leopotam.Ecs;
using Modules.Joystick.Data;
using Modules.UserInput;
using UnityEngine;

namespace Modules.Joystick.Systems
{
    public class JoystickSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter<OnScreenTapUp> _up = null;
        private readonly EcsFilter<OnScreenTapDown> _down = null;
        private readonly EcsFilter<OnScreenHold> _hold = null;
        private readonly EcsWorld _world = null;
        private readonly JoystickConfig _joystickConfig;
        
        private EcsEntity _joystickData;

        private Vector2 _input = Vector2.zero;
        private Vector3 _downPosition = Vector3.zero;

        public JoystickSystem(JoystickConfig joystickConfig)
        {
            _joystickConfig = joystickConfig;
        }

        public void Init()
        {
            _joystickData = _world.NewEntity();
            _joystickData.Get<JoystickData>();
        }

        public void Run()
        {
            if (!_joystickConfig.isEnabled)
            {
                _input = Vector2.zero;
            }
            else
            {
                if (_up.IsEmpty() == false)
                    _input = Vector2.zero;

                if (_down.IsEmpty() == false)
                    _downPosition = Input.mousePosition;

                if (_down.IsEmpty() == false || _hold.IsEmpty() == false)
                    Drag();
            }
            
            ref var data = ref _joystickData.Get<JoystickData>();
            data.InputDelta = data.Input - Direction;
            data.Input = Direction;
            data.BodyPosition = _downPosition;
        }

        private void Drag()
        {
            _input = (Input.mousePosition - _downPosition) / _joystickConfig.Radius;
            FormatInput();
            HandleInput(_input.magnitude, _input.normalized);
        }

        private void FormatInput()
        {
            if (_joystickConfig.axisOptions == AxisOptions.Horizontal)
                _input = new Vector2(_input.x, 0f);
            else if (_joystickConfig.axisOptions == AxisOptions.Vertical)
                _input = new Vector2(0f, _input.y);
        }

        protected void HandleInput(float magnitude, Vector2 normalised)
        {
            if (_joystickConfig.isDynamic && magnitude > _joystickConfig.MoveThreshold)
            {
                Vector3 difference = normalised * (magnitude - _joystickConfig.MoveThreshold) * _joystickConfig.Radius;
                _downPosition += difference;
            }

            if (magnitude > _joystickConfig.DeadZone)
            {
                if (magnitude > 1)
                    _input = normalised;
            }
            else
                _input = Vector2.zero;
        }

        public float Horizontal { get { return (_joystickConfig.SnapX) ? SnapFloat(_input.x, AxisOptions.Horizontal) : _input.x; } }
        public float Vertical { get { return (_joystickConfig.SnapY) ? SnapFloat(_input.y, AxisOptions.Vertical) : _input.y; } }
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (_joystickConfig.axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(_input, Vector2.up);
                if (snapAxis == AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }
            return 0;
        }
    }
}
