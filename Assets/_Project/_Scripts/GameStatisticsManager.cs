using MG_Utilities;

namespace EternalDefenders
{
    public class GameStatisticsManager : Singleton<GameStatisticsManager>
    {
        public int TowersDestroyed { get; private set; }
        public int PlayerDeaths { get; private set; }
        public int EnemiesKilled { get; private set; }
        //later we will add more etc.
        
        void Start()
        {
            TowersDestroyed = 0;
            PlayerDeaths = 0;
            EnemiesKilled = 0;
            
            SubscribeToEvents();
        }
        
        //TODO manage cleanup (unlinking on destroy)
        void SubscribeToEvents()
        {
            TowerController.OnTowerDestroyed += _ => TowersDestroyed++;
            PlayerController.Instance.OnPlayerDeath += () => PlayerDeaths++;
        }
    }
}