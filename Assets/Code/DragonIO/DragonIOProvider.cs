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
                
                // player
                .Add(new Player.Systems.PlayerInitSystem())
                .Add(new Player.Systems.PlayerMoveProcessing())

                // event group
                .Add(new EventGroup.StateCleanupSystem())       // remove entity with prev state component
                .Add(new EventHandlers.OnRestartRoundEnter())   // on click at restart button
                .Add(new EventHandlers.OnNextLevelEnter())      // start next level
                .Add(new EventHandlers.OnGamePlayStateEnter())  // enter at gameplay stage
                .Add(new EventHandlers.OnRoundCompletedEnter()) // on round completed state enter
                .Add(new EventHandlers.OnRoundFailedEnter())    // on round failed state enter

                .Add(new Utils.TimedDestructorSystem())
                
                // injections
                .Inject(_config)
                ;

            endFrame
                .OneFrame<EventGroup.StateEnter>()
                .OneFrame<Player.Components.PlayerSpawnedSignal>()
                ;

            return systems;
        }
    }
}
