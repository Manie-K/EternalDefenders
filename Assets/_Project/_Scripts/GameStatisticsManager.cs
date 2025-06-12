using MG_Utilities;
using System;

namespace EternalDefenders
{
    public class GameStatisticsManager : Singleton<GameStatisticsManager>
    {
        public int TowersDestroyed { get; private set; }
        public int PlayerDeaths { get; private set; }
        public int EnemiesKilled { get; private set; }
        public int Score {  get; private set; }
        //later we will add more etc.
        public event Action OnEnemyDead;
        void Start()
        {
            TowersDestroyed = 0;
            PlayerDeaths = 0;
            EnemiesKilled = 0;
            
            SubscribeToEvents();
        }

        public int GetFinalScore()
        {
            int gameLengthScore = (int)(GameManager.Instance.GameLength * 60 - GameManager.Instance.EndGameTimeRemaining);
            return TowersDestroyed * -100 + PlayerDeaths * -1000 + EnemiesKilled * 100 + gameLengthScore;
        }

        public void NotifyEnemyKilled()
        {
            EnemiesKilled++;
            OnEnemyDead?.Invoke();
        }
        
        //TODO manage cleanup (unlinking on destroy)
        void SubscribeToEvents()
        {
            TowerController.OnTowerDestroyed += (_) => TowersDestroyed++;
            PlayerController.Instance.OnDeath += () => PlayerDeaths++;
        }
    }
}