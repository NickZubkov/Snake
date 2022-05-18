using Leopotam.Ecs;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemyInitSystem : IEcsRunSystem
    {
        private EcsFilter<Dragons.Components.DragonHead, Components.Enemy, Components.EnemyHeadSpawnedSignal> _enemy;
            
        private Data.GameConfig _config;
        private EcsWorld _world;

        public void Run()
        {
            if(_enemy.IsEmpty())
                return;
            
            foreach (var idx in _enemy)
            {
                ref var enemy = ref _enemy.Get2(idx);
                _enemy.Get1(idx).Config = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).EnemyConfig;
                enemy.Config = _config.LevelsConfig.SafeGetAt(PlayerLevel.ProgressionInfo.CurrentLevel).EnemyConfig;
                enemy.ChangeDirectionTimeThreshold = enemy.Config.TimeToChangeDirection;
                enemy.ChangeDirectionTimer = 0;
                enemy.SerchRadiusThreshold = enemy.Config.SerchRadiusThreshold;
            }
        }
    }
}