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

        public AchieveAnalytic()
        {
            _firebaseAnalytic = new FirebaseAnalytic();
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
                PlayerPrefs.SetInt(DestroyedProgressKey, ++_currentProgress);
            }
        }

        public event Action<int> EnemiesPoints;
    }
}