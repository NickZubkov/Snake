﻿using Leopotam.Ecs;

namespace Modules.DragonIO.Player.Systems
{
    public class PlayerInitSystem : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, ViewHub.UnityView, Components.PlayerSpawnedSignal> _player;
            
        private Data.GameConfig _config;
        private EcsWorld _world;

        public void Run()
        {
            if(_player.IsEmpty())
                return;


            foreach (var idx in _player)
            {
                ref var player = ref _player.Get1(idx);
                player.Config = _config.DragonConfigs.SafeGetAt(0);
                
                _world.NewEntity().Get<CameraUtils.SwitchCameraSignal>() = new CameraUtils.SwitchCameraSignal
                {
                    CameraId = 0,
                    Follow = _player.Get2(idx).Transform
                };
            }
        }
    }
}