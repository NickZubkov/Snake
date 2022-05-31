using Leopotam.Ecs;
using UnityEngine;

namespace Modules.DragonIO.Enemy.Systems
{
    public class EnemyPathCalculateProcessing : IEcsRunSystem
    {
        private EcsFilter<EventGroup.GamePlayState> _gameplay;
        private EcsFilter<Dragons.Components.DragonHead, Components.Enemy> _enemy;
        private EcsFilter<LevelController.Components.LevelRunTimeData, LevelController.Components.CurrentLevelConfigs> _levelData;
        private Utils.TimeService _time;

        public void Run()
        {
            if (_gameplay.IsEmpty())
                return;

            foreach (var levelData in _levelData)
            {
                foreach (var idx in _enemy)
                {
                    ref var levelRunTimeData = ref _levelData.Get1(levelData);
                    
                    ref var dragonHead = ref _enemy.Get1(idx);
                    ref var enemy = ref _enemy.Get2(idx);

                    enemy.ChangeDirectionTimer -= _time.DeltaTime;
                    switch (enemy.EnemyAI)
                    {
                        case Data.EnemyAI.Easy: 
                            CalculateEasyAIDirection(ref enemy, ref dragonHead);
                            break;
                        case Data.EnemyAI.Medium:
                            CalculateMediumAIDirection(ref enemy, ref dragonHead, ref levelRunTimeData);
                            break;
                        case Data.EnemyAI.Hard:
                            CalculateHardAIDirection(ref enemy, ref dragonHead, ref levelRunTimeData);
                            break;
                        case Data.EnemyAI.Legend:
                            CalculateLegendAIDirection(ref enemy, dragonHead);
                            break;
                        default:
                            CalculateEasyAIDirection(ref enemy, ref dragonHead);
                            break;
                    }
                }  
            }
        }

        private void CalculateEasyAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead)
        {
            if (enemy.ChangeDirectionTimer <= 0f)
            {
                var randomInput = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                dragonHead.TargetHeadDirection = new Vector3(randomInput.x, 0f, randomInput.y);
                enemy.ChangeDirectionTimer = enemy.TimeToChangeDirection + Random.Range(0f, 1f);
            }
        }
        private void CalculateMediumAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, ref LevelController.Components.LevelRunTimeData runTimeData)
        {
            bool goodsFounded = false;

            Collider[] hitColliders = new Collider[runTimeData.MaxGoodsSerchingCount];
            Physics.OverlapSphereNonAlloc(dragonHead.HeadTransform.position, enemy.GoodsSerchRadius, hitColliders, runTimeData.GoodsLayerMask);
            var target = hitColliders[0];
            foreach (var collider in hitColliders)
            {
                if (collider == null)
                    continue;
                
                var newDirection = collider.transform.position - dragonHead.HeadTransform.position;
                var prevDirection = target.transform.position - dragonHead.HeadTransform.position;
                if (newDirection.sqrMagnitude < prevDirection.sqrMagnitude)
                {
                    target = collider;
                    dragonHead.TargetHeadDirection = newDirection.Where(y: 0f).normalized;
                    goodsFounded = true;
                }
            }

            if (!goodsFounded)
            {
                CalculateEasyAIDirection(ref enemy, ref dragonHead);
            }
        }
        private void CalculateHardAIDirection(ref Components.Enemy enemy, ref Dragons.Components.DragonHead dragonHead, ref LevelController.Components.LevelRunTimeData runTimeData)
        {
            RaycastHit Hit;
            var angle = 135f;
            bool leftHited = false;
            bool rightHited = false;
            bool centrHited = false;
            if (Physics.Raycast(dragonHead.HeadTransform.TransformPoint(new Vector3(-0.55f, 0f, 0.55f)), dragonHead.HeadTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (dragonHead.HeadTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }
                
                leftHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * -angle;
                    var newX = dragonHead.HeadTransform.position.x * Mathf.Cos(angle) - dragonHead.HeadTransform.position.z * Mathf.Sin(angle);
                    var newZ = dragonHead.HeadTransform.position.x * Mathf.Sin(angle) + dragonHead.HeadTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
            }
            else if (Physics.Raycast(dragonHead.HeadTransform.TransformPoint(new Vector3(0f, 0f, 0.55f)), dragonHead.HeadTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (dragonHead.HeadTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }

                centrHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * -angle;
                    var newX = dragonHead.HeadTransform.position.x * Mathf.Cos(angle) - dragonHead.HeadTransform.position.z * Mathf.Sin(angle);
                    var newZ = dragonHead.HeadTransform.position.x * Mathf.Sin(angle) + dragonHead.HeadTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
                        
            }
            else if (Physics.Raycast(dragonHead.HeadTransform.TransformPoint(new Vector3(0.55f, 0f, 0.55f)), dragonHead.HeadTransform.TransformDirection(Vector3.forward), out Hit, enemy.ObstacleSerchingDistance, enemy.ObstacleLayerMask))
            {
                if (Hit.collider.transform.parent != null)
                {
                    if (dragonHead.HeadTransform.parent.gameObject == Hit.collider.transform.parent.gameObject)
                    {
                        return;
                    }
                }

                rightHited = true;
                if (!enemy.IsAvoidingObstacle)
                {
                    angle = Mathf.Deg2Rad * angle;
                    var newX = dragonHead.HeadTransform.position.x * Mathf.Cos(angle) - dragonHead.HeadTransform.position.z * Mathf.Sin(angle);
                    var newZ = dragonHead.HeadTransform.position.x * Mathf.Sin(angle) + dragonHead.HeadTransform.position.z * Mathf.Cos(angle);
                    var newDirection = new Vector3(newX, 0f, newZ);
                    dragonHead.TargetHeadDirection = newDirection.normalized;
                }
                        
            }

            enemy.IsAvoidingObstacle = leftHited || rightHited || centrHited;
            
            if(!enemy.IsAvoidingObstacle)
            {
                CalculateMediumAIDirection(ref enemy, ref dragonHead, ref runTimeData);
            }
            
        }
        private void CalculateLegendAIDirection(ref Components.Enemy enemy, Dragons.Components.DragonHead head)
        {
            
        }
        
    }
}
