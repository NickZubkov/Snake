using Leopotam.Ecs;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerInitSystem : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, ViewHub.UnityView, Components.PlayerHeadSpawnedSignal> _player;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
            
        private EcsWorld _world;

        public void Run()
        {
            if(_player.IsEmpty())
                return;
            
            foreach (var idx in _player)
            {
                foreach (var levelController in _levelController)
                {
                    ref var controller = ref _levelController.Get1(levelController);
                    _player.Get1(idx).DragonConfig = controller.LevelsConfigs.PlayerConfig;
                
                    _world.NewEntity().Get<CameraUtils.SwitchCameraSignal>() = new CameraUtils.SwitchCameraSignal
                    {
                        CameraId = 0,
                        Follow = _player.Get2(idx).Transform
                    };
                }
                
            }
        }
    }
}