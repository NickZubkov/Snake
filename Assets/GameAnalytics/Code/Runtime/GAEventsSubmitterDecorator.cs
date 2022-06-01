using System;
using System.Collections.Generic;
using GameAnalyticsSDK;
using UAnalytics.EventSubmitter;

namespace UAnalytics.Integrations.GameAnalytics
{
    public class GAEventsSubmitterDecorator : EventSubmitterDecorator
    {
        private readonly Dictionary<string, Action<Dictionary<string, object>>> _customProcessors;
        private readonly Dictionary<string, GAAdType> _adTypeMapping;

        public GAEventsSubmitterDecorator()
        {
            _customProcessors = new Dictionary<string, Action<Dictionary<string, object>>>();

            _adTypeMapping = new Dictionary<string, GAAdType>()
            {
                { UAnalytics.AnalyticsFacade.DefaultEventCodes.AdTypeValues.Banner, GAAdType.Banner },
                { UAnalytics.AnalyticsFacade.DefaultEventCodes.AdTypeValues.Intersitial, GAAdType.Interstitial },
                { UAnalytics.AnalyticsFacade.DefaultEventCodes.AdTypeValues.Rewarded, GAAdType.RewardedVideo }
            };

            // setup for custom processing of specific eventCode

            // ads
            _customProcessors.Add(UAnalytics.AnalyticsFacade.DefaultEventCodes.AdEventCodes.VideoAdsWatch,
                ProcessVideoAdsWatchEvent);

            // progression
            _customProcessors.Add(UAnalytics.AnalyticsFacade.DefaultEventCodes.LevelProgressionEventCodes.LevelStarted,
                ProcessLevelStartedEvent);
            _customProcessors.Add(
                UAnalytics.AnalyticsFacade.DefaultEventCodes.LevelProgressionEventCodes.LevelCompleted,
                ProcessLevelCompletedEvent);
            _customProcessors.Add(UAnalytics.AnalyticsFacade.DefaultEventCodes.LevelProgressionEventCodes.LevelFailed,
                ProcessLevelFailedEvent);
        }

        public override void SubmitEvent(string eventCode, Dictionary<string, object> args)
        {
            base.SubmitEvent(eventCode, args);

            if (_customProcessors.ContainsKey(eventCode))
            {
                _customProcessors[eventCode].Invoke(args);
            }
            else
            {
                MainThreadDispatcher.Enqueue(() => GameAnalyticsSDK.GameAnalytics.NewDesignEvent(eventCode, args));
            }
        }

        public override void SubmitEvent(string eventCode)
        {
            base.SubmitEvent(eventCode);
            MainThreadDispatcher.Enqueue(() => GameAnalyticsSDK.GameAnalytics.NewDesignEvent(eventCode));
        }

        #region Progression

        private void ProcessLevelStartedEvent(Dictionary<string, object> args)
        {
            ProcessProgressionEvent(GAProgressionStatus.Start, args);
        }

        private void ProcessLevelCompletedEvent(Dictionary<string, object> args)
        {
            ProcessProgressionEvent(GAProgressionStatus.Complete, args);
        }

        private void ProcessLevelFailedEvent(Dictionary<string, object> args)
        {
            ProcessProgressionEvent(GAProgressionStatus.Fail, args);
        }

        private void ProcessProgressionEvent(GAProgressionStatus status, Dictionary<string, object> args)
        {
            var levelNumberKey = UAnalytics.AnalyticsFacade.DefaultEventCodes.LevelProgressionPropertyCodes.LevelNumber;
            var level = args.ContainsKey(levelNumberKey) ? args[levelNumberKey] as int? ?? -1 : -1;
            MainThreadDispatcher.Enqueue(() =>
                GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(status, $"level_{level}", args));
        }

        #endregion

        #region Ads

        private void ProcessVideoAdsWatchEvent(Dictionary<string, object> args)
        {
            ProcessAdEvent(GAAdAction.Show, args);
        }

        private void ProcessAdEvent(GAAdAction action, Dictionary<string, object> args)
        {
            bool correctDataReceived = true;

            GAAdType adType = GAAdType.Undefined;
            if (args.ContainsKey(UAnalytics.AnalyticsFacade.DefaultEventCodes.AdPropertyCodes.AdType)
                && _adTypeMapping.ContainsKey(args[UAnalytics.AnalyticsFacade.DefaultEventCodes.AdPropertyCodes.AdType]
                    as string ?? string.Empty))
            {
                adType = _adTypeMapping[args[UAnalytics.AnalyticsFacade.DefaultEventCodes.AdPropertyCodes.AdType]
                    as string ?? string.Empty];
            }

            string adPlacement = "undefined";
            if (args.ContainsKey(UAnalytics.AnalyticsFacade.DefaultEventCodes.AdPropertyCodes.Placement)
                && args[UAnalytics.AnalyticsFacade.DefaultEventCodes.AdPropertyCodes.Placement] is string placement)
            {
                adPlacement = placement;
            }

            MainThreadDispatcher.Enqueue(() =>
                GameAnalyticsSDK.GameAnalytics.NewAdEvent(action, adType, "adsSDK", adPlacement));
        }

        #endregion
    }
}