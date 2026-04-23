using System.Collections.Generic;
using AdjustSdk;
using UnityEngine;

namespace GameServices
{
    public class AdjustAnalytic : MonoBehaviour
    {
        [SerializeField] private string _appToken = "demo";

        private Dictionary<string, string> _adjustTokens = new Dictionary<string, string>()
        {
            { "enemy_destroyed", "test" },
        };

        private void Start()
        {
            AdjustConfig adjustConfig = new AdjustConfig(_appToken, AdjustEnvironment.Sandbox);

            adjustConfig.LogLevel = AdjustLogLevel.Verbose;
            adjustConfig.IsDeferredDeeplinkOpeningEnabled = true;
            adjustConfig.IsSendingInBackgroundEnabled = true;
            Adjust.InitSdk(adjustConfig);
            
            DontDestroyOnLoad(this);
        }

        public void SendEnemyDiedEvent(int points)
        {
            string eventName = "enemy_destroyed";
            AdjustEvent adjustEvent = new AdjustEvent(_adjustTokens.GetValueOrDefault(eventName));
            adjustEvent.AddCallbackParameter("points:", points.ToString());
            Adjust.TrackEvent(adjustEvent);
        }
    }
}