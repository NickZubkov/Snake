using GameAnalyticsSDK.Events;
using UAnalytics.EventSubmitter;
using UnityEngine;

namespace UAnalytics.Integrations.GameAnalytics
{
    [CreateAssetMenu(menuName = "UAnalytics/Integrations/GameAnalytics/EventSubmitterDecoratorFactory")]
    public class GAEventSubmitterDecoratorFactory : ASOEventSubmitterDecoratorFactory
    {
        public override EventSubmitterDecorator CreateSubmitter()
        {
            GameObject gaObject = new GameObject("GameAnalytics");
            gaObject.AddComponent<GameAnalyticsSDK.GameAnalytics>();
            gaObject.AddComponent<GA_SpecialEvents>();
            GameObject.DontDestroyOnLoad(gaObject);
            
            GameAnalyticsSDK.GameAnalytics.Initialize();
            
            return new GAEventsSubmitterDecorator();
        }
    }
}