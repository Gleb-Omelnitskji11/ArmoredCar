using System.Collections.Generic;
using AdjustSdk;
using UnityEngine;

namespace GameServices
{
    public class AdjustAnalytic : MonoBehaviour
    {
        private Dictionary<string, string> _adjustTokens = new Dictionary<string, string>()
        {
            { "enemy_destroyed", "test" },
        };

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        public void SendEnemyDiedEvent(int points)
        {
//#if UNITY_EDITOR
            return;
//#endif
            string eventName = "enemy_destroyed";
            AdjustEvent adjustEvent = new AdjustEvent(_adjustTokens.GetValueOrDefault(eventName));
            adjustEvent.AddCallbackParameter("points:", points.ToString());
            Adjust.TrackEvent(adjustEvent);
        }
    }
}