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
            DontDestroyOnLoad(this);
        }

        public void Init()
        {
#if UNITY_EDITOR
            return;
#endif
            AdjustConfig adjustConfig = new AdjustConfig(_appToken, AdjustEnvironment.Sandbox);

            adjustConfig.LogLevel = AdjustLogLevel.Info;
            adjustConfig.IsDeferredDeeplinkOpeningEnabled = true;
            adjustConfig.IsSendingInBackgroundEnabled = true;
            Adjust.InitSdk(adjustConfig);
        }

        public void SendEnemyDiedEvent(int points)
        {
#if UNITY_EDITOR
            return;
#endif
            string eventName = "enemy_destroyed";
            AdjustEvent adjustEvent = new AdjustEvent(_adjustTokens.GetValueOrDefault(eventName));
            adjustEvent.AddCallbackParameter("points:", points.ToString());
            Adjust.TrackEvent(adjustEvent);
        }
    }
}