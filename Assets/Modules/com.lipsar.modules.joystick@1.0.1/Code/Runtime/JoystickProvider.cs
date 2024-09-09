using Leopotam.Ecs;
using Modules.Root.ECS;
using UnityEngine;

namespace Modules.Joystick
{
    [CreateAssetMenu(fileName = "JoystickProvider", menuName = "Modules/Joystick/JoystickProvider")]
    public class JoystickProvider : ScriptableObject, ISystemsProvider
    {
        public Data.JoystickConfig joystickConfig;

        public EcsSystems GetSystems(EcsWorld world, EcsSystems endFrame, EcsSystems mainSystems)
        {
            EcsSystems systems = new EcsSystems(world, "JoystickSystems");

            systems
                .Add(new Systems.JoystickSystem(joystickConfig))
                .Add(new UI.JoysitckPanelDrawer(joystickConfig))
                ;

            return systems;
        }
    }
}