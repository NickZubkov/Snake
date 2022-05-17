using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemyPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private Utils.TimeService _time;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;
            
            foreach (var idx in _enemy)
            {
                ref var dragonHeadTransform = ref _enemy.Get1(idx).Transform;
                ref var dragonHead = ref _enemy.Get2(idx);
                ref var enemy = ref _enemy.Get3(idx);
                
                enemy.ChangeDirectionTimer -= _time.DeltaTime;
                
                if (enemy.ChangeDirectionTimer <= 0f)
                {
                    var randomInput = new Vector2(Random.Range(-0.5f, 0.6f), Random.Range(-0.5f, 0.6f));
                    enemy.MoveDirection = new Vector3(randomInput.x, 0f, randomInput.y);
                    enemy.ChangeDirectionTimer = enemy.ChangeDirectionTimeThreshold + Random.Range(0f, 1f);
                }

                // Calc head position
                Vector3 targetHeadPoint = dragonHeadTransform.position + enemy.MoveDirection * _time.DeltaTime * enemy.Config.Speed;

                // Store position history
                dragonHead.PositionsHistory.Insert(0, targetHeadPoint);

                // Clear position history
                int targetCount = dragonHead.BodyParts.Count * enemy.Config.Gap;
                if (dragonHead.PositionsHistory.Count > targetCount)
                {
                    var countToRemove = dragonHead.PositionsHistory.Count - targetCount;
                    dragonHead.PositionsHistory.RemoveRange(targetCount, countToRemove);
                }
                
            }
        }
    }
}