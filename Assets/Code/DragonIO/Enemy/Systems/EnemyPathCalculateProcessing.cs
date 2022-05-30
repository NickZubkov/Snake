using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemyPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<ViewHub.UnityView, Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
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
                switch (enemy.EnemyAI)
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
        private void CalculateMediumAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, Transform headTransform, Vector2 input)
        {
            bool goodsFounded = false;

            Collider[] hitColliders = new Collider[enemy.MaxGoodsSerchingCount];
            Physics.OverlapSphereNonAlloc(headTransform.position, enemy.GoodsSerchRadius, hitColliders, enemy.GoodslayerMask);
            var target = hitColliders[0];
            foreach (var collider in hitColliders)
            {
                if (collider == null)
                    continue;
                
                var newDirection = collider.transform.position - headTransform.position;
                var prevDirection = target.transform.position - headTransform.position;
                if (newDirection.sqrMagnitude < prevDirection.sqrMagnitude)
                {
                    target = collider;
                    dragonHead.TargetHeadDirection = newDirection.Where(y: 0f).normalized;
                    goodsFounded = true;
                }
            }

            if (!goodsFounded)
            {
                CalculateEasyAIDirection(ref enemy, ref dragonHead, input);
            }
        }
        private void CalculateHardAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, Transform headTransform, EcsEntity entity, Vector2 input)
        {
            RaycastHit Hit;
            var angle = 135f;
            bool leftHited = false;
            bool rightHited = false;
            bool centrHited = false;
            if (Physics.Raycast(headTransform.TransformPoint(new Vector3(-0.55f, 0f, 0.55f)), headTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (headTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }
                
                leftHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * -angle;
                    var newX = headTransform.position.x * Mathf.Cos(angle) - headTransform.position.z * Mathf.Sin(angle);
                    var newZ = headTransform.position.x * Mathf.Sin(angle) + headTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
            }
            else if (Physics.Raycast(headTransform.TransformPoint(new Vector3(0f, 0f, 0.55f)), headTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (headTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }

                centrHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * -angle;
                    var newX = headTransform.position.x * Mathf.Cos(angle) - headTransform.position.z * Mathf.Sin(angle);
                    var newZ = headTransform.position.x * Mathf.Sin(angle) + headTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
                        
            }
            else if (Physics.Raycast(headTransform.TransformPoint(new Vector3(0.55f, 0f, 0.55f)), headTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (headTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }

                rightHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * angle;
                    var newX = headTransform.position.x * Mathf.Cos(angle) - headTransform.position.z * Mathf.Sin(angle);
                    var newZ = headTransform.position.x * Mathf.Sin(angle) + headTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
                        
            }

            enemy.IsAvoidingObstacle = leftHited || rightHited || centrHited;
            
            if(!enemy.IsAvoidingObstacle)
            {
                CalculateMediumAIDirection(ref enemy, ref dragonHead, headTransform, input);
            }
            
        }
        private void CalculateLegendAIDirection(ref Components.Enemy enemy, Transform headPosition, Dragons.Components.DragonHead head)
        {
            
        }
        
    }
}
