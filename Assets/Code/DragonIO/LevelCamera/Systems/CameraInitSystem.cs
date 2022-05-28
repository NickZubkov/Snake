using Leopotam.Ecs;

namespace Modules.DragonIO.LevelCamera.Systems
{
    public class CameraInitSystem : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Player.Components.Player> _player;
        private EcsFilter<LevelController.Components.PlayerSpawningSignal> _signal;

        private EcsWorld _world;
        public void Run()
        {
            if (!_signal.IsEmpty())
            {
                foreach (var player in _player)
                {
                    _world.NewEntity().Get<CameraUtils.SwitchCameraSignal>() = new CameraUtils.SwitchCameraSignal
                    {
                        CameraId = 0,
                        Follow = _player.Get1(player).Transform
                    };
                }
            }
        }
    }
}