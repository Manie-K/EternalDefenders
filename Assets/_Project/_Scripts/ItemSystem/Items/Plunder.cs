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
                CollectResources();
            }
            UpdateItemCost(true);
        }

        public override void Remove()
        {
            DuplicateCount--;

            if (DuplicateCount == 0)
            {
                CollectResources();
            }
            UpdateItemCost(false);
        }

        public void CollectResources()
        {

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
