using Leopotam.Ecs;
using Modules.Root.ECS;
using Modules.Root.ContainerComponentModel;
using UnityEngine;

namespace Modules.DragonIO
{
    [CreateAssetMenu(menuName = "Modules/DragonIO/Provider")]
    public class DragonIOProvider : ScriptableObject, ISystemsProvider
    {
        [SerializeField] private Data.GameConfig _config;
        public EcsSystems GetSystems(EcsWorld world, EcsSystems endFrame, EcsSystems mainSystems)
        {
            EcsSystems systems = new EcsSystems(world, "DragonIOGame");

            #region AppContainerCheck
            if (AppContainer.Instance == null) 
            {
                // wrong behavior
                // app container not initialized, handle via init scene load
                Debug.LogWarning(
                    "<color=darkblue>CommonTemplate:</color> App container not initialized, resolved via init scene load\n" +
                    "LOOK AT: http://youtrack.lipsar.studio/articles/LS-A-149/App-container-not-initialized"
                    );

#if UNITY_IOS || UNITY_ANDROID
                Handheld.Vibrate();
#endif

                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                return systems;
            }
            #endregion

            systems
                .Add(new GameInit())
                
                // level controller
                .Add(new LevelController.Systems.LevelControllerInitSystem())
                .Add(new LevelController.Systems.LevelObjectsCountProcessing())
                
                // location
                .Add(new Location.Systems.LocationGroundInitSystem())
                .Add(new Location.Systems.LocationWallsSpawnProcessing())
                .Add(new Location.Systems.ObstaclesSpawnProcessing())
                .Add(new Location.Systems.GroundDecorSpawnProcessing())
                
                // player
                .Add(new Player.Systems.PlayerSpawnSystem())
                .Add(new Player.Systems.PlayerInitSystem())
                .Add(new Player.Systems.PlayerPathCalculateProcessing())
                
                // enemy
                .Add(new Enemy.Systems.EnemySpawnSystem())
                .Add(new Enemy.Systems.EnemyPathCalculateProcessing())

                // dragons
                .Add(new Dragons.Systems.DragonsCollectGoodsProcessing())
                .Add(new Dragons.Systems.DragonBodySpawnProcessing())
                .Add(new Dragons.Systems.DragonScalingProcessing())
                .Add(new Dragons.Systems.DragonsCollisionsProcessing())
                .Add(new Dragons.Systems.DragonsMoveProcessing())
                
                // goods
                .Add(new Goods.Systems.GoodsSpawnProcessing())
                .Add(new Goods.Systems.GoodsEffectsProcessing())
                
                // camera
                .Add(new LevelCamera.Systems.CameraInitSystem())
                .Add(new LevelCamera.Systems.CameraOffsetProcessing())
                
                // ui
                .Add(new UI.Systems.UIUpdateProcessing())
                
                // utils
                .Add(new Utils.TimedDestructorSystem())
                
                // round end tracker
                .Add(new RoundCompletedTracker())
                .Add(new RoundFailedTracker())

                // event group
                .Add(new EventGroup.StateCleanupSystem())       // remove entity with prev state component
                .Add(new EventHandlers.OnRestartRoundEnter())   // on click at restart button
                .Add(new EventHandlers.OnNextLevelEnter())      // start next level
                .Add(new EventHandlers.OnGamePlayStateEnter())  // enter at gameplay stage
                .Add(new EventHandlers.OnRoundCompletedEnter()) // on round completed state enter
                .Add(new EventHandlers.OnRoundFailedEnter())    // on round failed state enter
                
                // injections
                .Inject(_config)
                ;

            endFrame
                .OneFrame<EventGroup.StateEnter>()
                .OneFrame<LevelController.Components.GoodsSpawningSignal>()
                .OneFrame<LevelController.Components.EnemySpawningSignal>()
                .OneFrame<LevelController.Components.ObstaclesSpawningSignal>()
                .OneFrame<LevelController.Components.GroundDecorSpawningSignal>()
                .OneFrame<LevelController.Components.WallSpawningSignal>()
                .OneFrame<LevelController.Components.PlayerSpawningSignal>()
                .OneFrame<LevelController.Components.DragonBodySpawningSignal>()
                .OneFrame<LevelController.Components.LevelComplitedSignal>()
                .OneFrame<LevelController.Components.LevelFaildSignal>()
                .OneFrame<Dragons.Components.DragonHeadSpawnedSignal>()
                .OneFrame<Goods.Components.PlayBonusVFXSignal>()
                .OneFrame<Goods.Components.PlayWinVFXSignal>()
                .OneFrame<Goods.Components.PlayDeathVFXSignal>()
                .OneFrame<Goods.Components.StopPowerUpVFXSignal>()
                
                ;

            return systems;
        }
    }
}
