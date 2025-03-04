using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ExampleItem", menuName = "EternalDefenders/ItemSystem/Items/ExampleItem")]
    public class ExampleItem : Item
    {
    
        public ExampleItem()
        {
            TowerController.OnTowerDestroyed += HandleTowerDestroyed;
        }

        private void HandleTowerDestroyed(TowerController towerController, TowerEventArgs args)
        {
            Debug.Log("Tower dustruction prevented");

            args.ShouldPreventDestruction = true;
            towerController.Stats.SetStat(StatType.Health, 1000);
            towerController.Stats.SetStat(StatType.Cooldown, 1);
        }

    }
}
