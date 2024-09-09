using UnityEngine;

namespace Modules.Joystick.Data
{
    public enum AxisOptions { Both, Horizontal, Vertical }

    [CreateAssetMenu(menuName = "Modules/Joystick/Data/JoystickConfig", fileName = "JoystickConfig")]
    public class JoystickConfig : ScriptableObject
    {
        [Tooltip("If disabled, UI joystick will not appear, and joystick will not take input.")]
        public bool isEnabled = true;
    
        [Header("Joystick Settings")]
        [Tooltip("The distance the visual handle can move from the center of the joystick.")]
        public float HandleRange = 1;
        [Tooltip("The distance away from the center input has to be before registering.")]
        public float DeadZone = 0;
        [Tooltip("Which axes the joystick uses.")]
        public AxisOptions axisOptions = AxisOptions.Both;
        [Tooltip("Snap the horizontal input to a whole value.")]
        public bool SnapX = false;
        [Tooltip("Snap the vertical input to a whole value.")]
        public bool SnapY = false;
        [Tooltip("The radius of the joystick")]
        public float Radius = 128.0f;

        [Header("Dynamic Joystick Settings")]
        [Tooltip("Will the joystick body follow a finger?")]
        public bool isDynamic;
        [Tooltip("The distance away from the center input has to be before the joystick begins to move.")]
        public float MoveThreshold;
    }
}
