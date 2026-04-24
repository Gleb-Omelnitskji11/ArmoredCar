using Firebase.Analytics;

namespace GameServices
{
    public class FirebaseAnalytic
    {
        
        
        public void SendEnemyDiedEvent(int points)
        {
            Parameter[] parameters = new Parameter[1];
            parameters[0] = new Parameter("points:", points.ToString());
            FirebaseAnalytics.LogEvent("enemy_destroyed", parameters);
        }
    }
}