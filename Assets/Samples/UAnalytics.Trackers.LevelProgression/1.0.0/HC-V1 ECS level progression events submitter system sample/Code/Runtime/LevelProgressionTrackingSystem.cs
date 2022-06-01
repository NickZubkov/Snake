using Leopotam.Ecs;
using Modules.EventGroup;
using Modules.LevelSpawner;

namespace UAnalytics.Samples.LevelProgressionTracker
{
    // place right before event group systems in hc-stack-v1
    // adapted for hc-v1-common template
    public class LevelProgressionAnalyticsEventsTracker : IEcsRunSystem
    {
        private readonly EcsFilter<LevelSpawnedSignal> _levelSpawnedSignal;
        private readonly EcsFilter<AddressableLevelAsset> _addresableLevelAsset;

        private readonly EcsFilter<GamePlayState, StateEnter> _onEnterGamePlay;
        private readonly EcsFilter<RoundCompletedState, StateEnter> _onRoundCompleted;
        private readonly EcsFilter<RoundFailedState, StateEnter> _onRoundFailed;
        private readonly EcsFilter<RestartRoundState, StateEnter> _onRoundRestart;
        private readonly EcsFilter<NextLevelState, StateEnter> _onNextLevelEnter;


        public void Run()
        {
            if (!_onNextLevelEnter.IsEmpty())
            {
                // update level number
                // +1 to start indexing levels in analytics from 1 not from 0
                Trackers.LevelProgressionTracker.SetCurrentLevelNumber(Modules.PlayerLevel.ProgressionInfo.CurrentLevel+1);
            }
            
            if (!_levelSpawnedSignal.IsEmpty())
            {
                // update level name
                // example works with addresable level spawner
                foreach (var i in _addresableLevelAsset)
                {
                    Trackers.LevelProgressionTracker.SetCurrentLevelName(_addresableLevelAsset.Get1(i).TargetAddres);
                }
                
                // todo set CurrentLocationName if game supports locations
                Trackers.LevelProgressionTracker.SetCurrentLocationName("non_specified");
                
                // todo set CurrentLevelType if game has more than 1 level type [bonus, regular]
                Trackers.LevelProgressionTracker.SetCurrentLevelType("regular");
            }

            if (!_onEnterGamePlay.IsEmpty())
            {
                // submit level started event once gamepeplay state started
                Trackers.LevelProgressionTracker.SubmitLevelStartedEvent();
            }
            
            if (!_onRoundCompleted.IsEmpty())
            {
                // submit level completed event while entering round completed state
                Trackers.LevelProgressionTracker.SubmitLevelCompletedEvent();
            }
            
            if (!_onRoundFailed.IsEmpty())
            {
                // submit level failed event while entering round failed state
                Trackers.LevelProgressionTracker.SubmitLevelFailedEvent();
            }
            
            if (!_onRoundRestart.IsEmpty())
            {
                // submit round restarted event while entering restart state
                Trackers.LevelProgressionTracker.SubmitLevelRestartedEvent();
            }
        }
    }
}