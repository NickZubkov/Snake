using Leopotam.Ecs;
using Modules.UserInput;
using UICoreECS;
using UnityEngine;

namespace Modules.Joystick.UI
{
    public class JoystickPanel : AUIEntity
    {
        [Tooltip("The joystick's Gameobject")]
        [SerializeField] public GameObject JoystickBody;
        [Tooltip("The background's RectTransform component.")]
        [SerializeField] private RectTransform Frame = default;
        [Tooltip("The handle's RectTransform component.")]
        [SerializeField] private RectTransform Handle = default;

        private RectTransform _baseRect = null;

        public override void Init(EcsWorld world, EcsEntity screen)
        {
            screen.Get<JoystickPanelView>().View = this;

            SwitchJoystickBody(false);
            _baseRect = GetComponent<RectTransform>();
        }

        public void SwitchJoystickBody(bool value)
        {
            JoystickBody.SetActive(value);
            if (value)
                MoveFrame(Input.mousePosition);
        }

        public void MoveHandle(Vector2 position)
        {
            Handle.anchoredPosition = position;
        }

        public void MoveFrame(Vector2 position)
        {
            Frame.anchoredPosition = ScreenPointToAnchoredPosition(position);
        }

        private Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_baseRect, screenPosition, null, out localPoint))
            {
                Vector2 pivotOffset = _baseRect.pivot * _baseRect.sizeDelta;
                return localPoint - (Frame.anchorMax * _baseRect.sizeDelta) + pivotOffset;
            }
            return localPoint;
        }
    }

    public struct JoystickPanelView
    {
        public JoystickPanel View;
    }

    public class JoysitckPanelDrawer : IEcsRunSystem
    {
        private readonly EcsFilter<JoystickPanelView> _view = null;
        private readonly EcsFilter<JoystickData> _joystick = null;
        private readonly EcsFilter<OnScreenTapUp> _up = null;
        private readonly EcsFilter<OnScreenTapDown> _down = null;
        private readonly EcsFilter<OnScreenHold> _hold = null; 

        private readonly Data.JoystickConfig _joystickConfig;

        public JoysitckPanelDrawer(Data.JoystickConfig joystickConfig)
        {
            _joystickConfig = joystickConfig;
        }

        public void Run()
        {
            if (_view.IsEmpty())
                return;

            foreach (var i in _view)
            {
                if (!_joystickConfig.isEnabled)
                {
                    _view.Get1(i).View.SwitchJoystickBody(false);
                    continue;
                }
                
                foreach (var j in _joystick)
                {
                    if (_down.IsEmpty() == false)
                        _view.Get1(i).View.SwitchJoystickBody(true);

                    if (_hold.IsEmpty() == false && _view.Get1(i).View.JoystickBody.activeSelf == false)
                        _view.Get1(i).View.SwitchJoystickBody(true);

                    _view.Get1(i).View.MoveHandle(_joystickConfig.Radius * _joystickConfig.HandleRange * _joystick.Get1(j).Input);
                    
                    if (_joystickConfig.isDynamic)
                        _view.Get1(i).View.MoveFrame(_joystick.Get1(i).BodyPosition);

                    if (_up.IsEmpty() == false)
                        _view.Get1(i).View.SwitchJoystickBody(false);
                }
            }
        }
    }
}
