using System;
using UnityEngine;

namespace GameServices
{
    public class AchieveAnalytic
    {
        private readonly int[] EnemiesDestroyedAchieves = new int[] { 10, 50, 100 };
        private const string DestroyedProgressKey = "DestroyedProgress";
        private int _currentProgress = -1;
        private FirebaseAnalytic _firebaseAnalytic;
        private AdjustAnalytic _adjustAnalytic;

        public AchieveAnalytic(AdjustAnalytic adjustAnalytic, FirebaseAnalytic firebaseAnalytic)
        {
            _adjustAnalytic = adjustAnalytic;
            _firebaseAnalytic = firebaseAnalytic;
        }

        public void AddEnemyDied(int points)
        {
            int nextAchieve = _currentProgress + 1;
            if(nextAchieve >= EnemiesDestroyedAchieves.Length)
                return;

            if (_currentProgress == -1)
            {
                _currentProgress = PlayerPrefs.GetInt(DestroyedProgressKey, -1);
            }
            
            int goal = EnemiesDestroyedAchieves[nextAchieve];
            
            if (points > goal)
            {
                EnemiesPoints?.Invoke(points);
                _firebaseAnalytic.SendEnemyDiedEvent(points);
                _adjustAnalytic.SendEnemyDiedEvent(points);
                PlayerPrefs.SetInt(DestroyedProgressKey, ++_currentProgress);
                PlayerPrefs.Save();
            }
        }

        public event Action<int> EnemiesPoints;
    }
}