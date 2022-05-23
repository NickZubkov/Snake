using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemyPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private EcsFilter<LevelController.Components.LevelController> _levelController;
        private Utils.TimeService _time;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var idx in _enemy)
            {
                ref var entity = ref _enemy.GetEntity(idx);
                ref var dragonHeadTransform = ref _enemy.Get1(idx).Transform;
                ref var dragonHead = ref _enemy.Get2(idx);
                ref var enemy = ref _enemy.Get3(idx);

                enemy.ChangeDirectionTimer -= _time.DeltaTime;
                var randomInput = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                switch (enemy.EnemyConfig.EnemyAI)
                {
                    case Data.EnemyAI.Easy: 
                        CalculateEasyAIDirection(ref enemy, ref dragonHead, randomInput);
                        break;
                    case Data.EnemyAI.Medium:
                        CalculateMediumAIDirection(ref enemy, ref dragonHead, dragonHeadTransform, randomInput);
                        break;
                    case Data.EnemyAI.Hard:
                        CalculateHardAIDirection(ref enemy, ref dragonHead, dragonHeadTransform, entity, randomInput);
                        break;
                    case Data.EnemyAI.Legend:
                        CalculateLegendAIDirection(ref enemy, dragonHeadTransform, dragonHead);
                        break;
                    default:
                        CalculateEasyAIDirection(ref enemy, ref dragonHead, randomInput);
                        break;
                }
            }
        }

        private void CalculateEasyAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, Vector2 input)
        {
            if (enemy.ChangeDirectionTimer <= 0f)
            {
                dragonHead.TargetHeadDirection = new Vector3(input.x, 0f, input.y);
                enemy.ChangeDirectionTimer = enemy.TimeToChangeDirection + Random.Range(0f, 1f);
            }
        }
        private void CalculateMediumAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, Transform headPosition, Vector2 input)
        {
            foreach (var levelController in _levelController)
            {
                bool goodsFounded = false;
                foreach (var goodsPosition in _levelController.Get1(levelController).GoodsPositions)
                {
                    var distance = Vector3.Distance(goodsPosition.position, headPosition.position);
                    
                    if (distance > enemy.SerchRadiusThreshold && distance < enemy.EnemyConfig.GoodsSerchRadius)
                    {
                        var MoveDirection = (goodsPosition.position - headPosition.position).normalized;
                        dragonHead.TargetHeadDirection = MoveDirection;
                        goodsFounded = true;
                        break;
                    }
                }

                if (!goodsFounded)
                {
                    CalculateEasyAIDirection(ref enemy, ref dragonHead, input);
                }
            }
        }
        private void CalculateHardAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, Transform headPosition, EcsEntity entity, Vector2 input)
        {
            RaycastHit Hit;
            if (Physics.Raycast(headPosition.TransformPoint(new Vector3(-0.55f, 0f, 0.55f)), headPosition.TransformDirection(Vector3.forward), out Hit, 8f))
            {
                if (Hit.transform.TryGetComponent(out ViewHub.EntityRef entityRef))
                {
                    if (entityRef.Entity.Has<Obstacles.Components.Obstacle>())
                    {
                        if (entityRef.Entity.Has<Dragons.Components.DragonBody>())
                        {
                            if (entity == entityRef.Entity.Get<Dragons.Components.DragonBody>().Head)
                            {
                                return;
                            }
                        }
                        var newDirection = new Vector3(-headPosition.position.z, 0f, headPosition.position.x);
                        dragonHead.TargetHeadDirection = newDirection.normalized;
                    }
                }
            }
            else if (Physics.Raycast(headPosition.TransformPoint(new Vector3(0.55f, 0f, 0.55f)), headPosition.TransformDirection(Vector3.forward), out Hit, 8f))
            {
                if (Hit.transform.TryGetComponent(out ViewHub.EntityRef entityRef))
                {
                    if (entityRef.Entity.Has<Obstacles.Components.Obstacle>())
                    {
                        if (entityRef.Entity.Has<Dragons.Components.DragonBody>())
                        {
                            if (entity == entityRef.Entity.Get<Dragons.Components.DragonBody>().Head)
                            {
                                return;
                            }
                        }
                       var newDirection = new Vector3(headPosition.position.z, 0f, -headPosition.position.x);
                        dragonHead.TargetHeadDirection = newDirection.normalized;
                    }
                }
            }
            else if (Physics.Raycast(headPosition.TransformPoint(new Vector3(0f, 0f, 0.55f)), headPosition.TransformDirection(Vector3.forward), out Hit, 8f))
            {
                if (Hit.transform.TryGetComponent(out ViewHub.EntityRef entityRef))
                {
                    if (entityRef.Entity.Has<Obstacles.Components.Obstacle>())
                    {
                        if (entityRef.Entity.Has<Dragons.Components.DragonBody>())
                        {
                            if (entity == entityRef.Entity.Get<Dragons.Components.DragonBody>().Head)
                            {
                                return;
                            }
                        }
                        var newDirection = new Vector3(-headPosition.position.z, 0f, headPosition.position.x);
                        dragonHead.TargetHeadDirection = newDirection.normalized;
                    }
                }
            }
            else
            {
                CalculateMediumAIDirection(ref enemy, ref dragonHead, headPosition, input);
            }
            
        }
        private void CalculateLegendAIDirection(ref Components.Enemy enemy, Transform headPosition, Dragons.Components.DragonHead head)
        {
            
        }
        
    }
}
