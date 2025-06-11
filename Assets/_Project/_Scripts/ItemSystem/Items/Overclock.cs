using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Overclock", menuName = "EternalDefenders/ItemSystem/Items/Overclock")]
    public class Overclock : Item
    {
        [SerializeField] private int _attackSpeedBoost = -2;
        [SerializeField] private int _priceChangeMutiplier = 2;
        [SerializeField] private int _priceChangeFlat = 50; 

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

            playerStats.ApplyModifier(modifier);

        }

        private void ChangePrice(bool wasDuplicateCountRaised)
        {
            if (wasDuplicateCountRaised)
            {
                foreach (ResourceCost resourceCost in _cost)
                {
                    resourceCost.amount = resourceCost.amount * _priceChangeMutiplier + _priceChangeFlat;
                }
            }
            else
            {
                foreach (ResourceCost resourceCost in _cost)
                {
                    resourceCost.amount = (resourceCost.amount - _priceChangeFlat) / _priceChangeMutiplier;
                }
            }

        }
    }
}
