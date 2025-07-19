using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Plunder", menuName = "EternalDefenders/ItemSystem/Items/Plunder")]
    public class Plunder : Item
    {
        [SerializeField] private int _resourceGainAmount;
        [SerializeField] private int _resourceGainDuplicateAmount;
        [SerializeField] private int _priceChangeFlat;
        public override void Collect()
        {
            DuplicateCount++;

            if (DuplicateCount == 1)
            {
                GameStatisticsManager.Instance.OnEnemyDead += CollectResources;
            }
            UpdateItemCost(true);
        }

        public override void Remove()
        {
            DuplicateCount--;

            if (DuplicateCount == 0)
            {
                GameStatisticsManager.Instance.OnEnemyDead -= CollectResources;
            }
            UpdateItemCost(false);
        }

        public void CollectResources()
        {
            var inventory = PlayerResourceInventory.Instance;

            int randomResource = Random.Range(0, Cost.Count);
            int resourceGainAmount = _resourceGainAmount + (DuplicateCount - 1) * _resourceGainDuplicateAmount;

            inventory.AddResource(Cost[randomResource].resource, resourceGainAmount);
        }

        public void UpdateItemCost(bool wasDuplicateCountRaised)
        {
            if (wasDuplicateCountRaised)
            {
                foreach (ResourceCost resourceCost in _cost)
                {
                    resourceCost.amount = resourceCost.amount + _priceChangeFlat;
                }
            }
            else
            {
                foreach (ResourceCost resourceCost in _cost)
                {
                    resourceCost.amount = resourceCost.amount - _priceChangeFlat;
                }
            }
        }
    }
}
