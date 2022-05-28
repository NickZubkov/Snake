using Leopotam.Ecs;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerInitSystem : IEcsRunSystem
    {
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.PlayerHeadSpawnedSignal> _player;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
            
        private EcsWorld _world;

        public void Run()
        {
            if(_player.IsEmpty())
                return;
            
            foreach (var player in _player)
            {
                foreach (var levelData in _levelData)
                {
                    ref var currentLevelConfigs = ref _levelData.Get2(levelData);
                    _player.Get2(player).DragonConfig = currentLevelConfigs.PlayerConfig;
                
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