using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    public class Overclock : Item
    {
        [SerializeField] private readonly int _attackSpeedBoost = -2;
        [SerializeField] private readonly int _priceChangeMutiplier = 2;
        [SerializeField] private readonly int _priceChangeFlat = 50;
        public override void Initialize(int id, string name)
        {
            List<TowerBundle.ResourceCost> cost = new() {
                new ResourceCost
                {
                    resource = new(),
                    amount = 50
                },
                new ResourceCost
                {
                    resource = new(),
                    amount = 50
                }
            };

            InitializeCommon(
                name: name,
                description: $"Boosts attack speed of a player",
                id: id,
                icon: null,
                rarity: Rarity.Uncommon,
                cost: cost,
                priority: 5,
                unique: false,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Player
            );

        }

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
