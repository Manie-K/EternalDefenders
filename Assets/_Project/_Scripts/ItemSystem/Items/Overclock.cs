using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Overclock", menuName = "EternalDefenders/ItemSystem/Items/Overclock")]
    public class Overclock : Item
    {
        [SerializeField] private int _attackSpeedBoost;
        [SerializeField] private int _priceChangeMutiplier;
        [SerializeField] private int _priceChangeFlat; 

        public override void Collect()
        {
            DuplicateCount++;

            ApplyStats(true);
            ChangePrice(true);

        }

        public override void Remove()
        {
            DuplicateCount--;

            ApplyStats(false);
            ChangePrice(false);
        }

        private void ApplyStats(bool wasDuplicateCountRaised)
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int attackSpeedBoost = wasDuplicateCountRaised ? _attackSpeedBoost : -_attackSpeedBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();

            modifier.statType = StatType.Cooldown;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = attackSpeedBoost;
            modifier.persistAfterFinish = true;
            modifier.limitedDurationTime = 0.01f;

            playerStats.ApplyModifier(modifier);

        }

        private void ChangePrice(bool wasDuplicateCountRaised)
        {
            var item = ItemManager.Instance.ItemDictionary.Items[Id];
            var cost = item._cost;
            if (wasDuplicateCountRaised)
            {
                foreach (TowerBundle.ResourceCost resourceCost in cost)
                {
                    resourceCost.amount = resourceCost.amount * _priceChangeMutiplier + _priceChangeFlat;
                }
            }
            else
            {
                foreach (TowerBundle.ResourceCost resourceCost in cost)
                {
                    resourceCost.amount = (resourceCost.amount - _priceChangeFlat) / _priceChangeMutiplier;
                }
            }

        }
    }
}
