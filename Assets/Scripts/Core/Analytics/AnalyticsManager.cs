namespace GameServices
{
    public class AnalyticsManager
    {
        private FirebaseAnalytic _firebaseAnalytic;
        private AdjustAnalytic _adjustAnalytic;

        public AnalyticsManager(AdjustAnalytic adjustAnalytic, FirebaseAnalytic firebaseAnalytic)
        {
            _adjustAnalytic = adjustAnalytic;
            _firebaseAnalytic = firebaseAnalytic;
        }

        public void SendEnemyDied(int points)
        {
            _firebaseAnalytic.SendEnemyDiedEvent(points);
            _adjustAnalytic.SendEnemyDiedEvent(points);
        }

        public void SendGameOver()
        {
        }
    }
}