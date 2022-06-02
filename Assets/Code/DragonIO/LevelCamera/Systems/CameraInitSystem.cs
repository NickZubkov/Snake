using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.LevelCamera.Systems
{
    public class CameraInitSystem : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, Player.Components.Player> _player;
        private EcsFilter<LevelController.Components.PlayerSpawningSignal> _playerSignal;
        private EcsFilter<LevelController.Components.ChangeCameraFollowSignal> _signal;
        private EcsFilter<CameraUtils.VirtualCamera> _camera;

        private EcsWorld _world;
        public void Run()
        {
            if (!_playerSignal.IsEmpty())
            {
                foreach (var player in _player)
                {
                    _world.NewEntity().Get<CameraUtils.SwitchCameraSignal>() = new CameraUtils.SwitchCameraSignal
                    {
                        CameraId = 0,
                        Follow = _player.Get1(player).HeadTransform,
                        LookAt = null
                    };

                    foreach (var camera in _camera)
                    {
                        var rot = _camera.Get1(camera).Camera.VirtualCameraGameObject.transform;
                        rot.eulerAngles = new Vector3(60, 0, 0);
                    }
                }
            }
            foreach (var idx in _signal)
            {
                if (_signal.Get1(idx).FollowTransform == null)
                    continue;
                
                _world.NewEntity().Get<CameraUtils.SwitchCameraSignal>() = new CameraUtils.SwitchCameraSignal
                {
                    CameraId = 0,
                    Follow = _signal.Get1(idx).FollowTransform,
                    LookAt = _signal.Get1(idx).FollowTransform
                };
            }
        }
    }
}