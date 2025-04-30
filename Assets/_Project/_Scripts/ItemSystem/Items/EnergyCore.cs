using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "EnergyCore", menuName = "EternalDefenders/ItemSystem/Items/EnergyCore")]
    public class EnergyCore : Item
    {

        [SerializeField] private int _speedBoost = 5;
        [SerializeField] private int _speedBoostPerDuplicate = 1;

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
                description: "Gives player movement speed boost",
                id: id,
                icon: null,
                rarity: Rarity.Rare,
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

            if (DuplicateCount == 1)
            {
                ApplyStats();
            }
            ApplyStatsDuplicate(true);
        }

        public override void Remove()
        {
            DuplicateCount--;

            if (DuplicateCount == 0)
            {
                ApplyStats();
            }
            ApplyStatsDuplicate(false);
        }

        private void ApplyStatsDuplicate(bool wasDuplicateCountRaised)
        {
            if (Mathf.Abs(DuplicateCount) > 1)
            {
                int speedBoostPerDuplicate = wasDuplicateCountRaised ? _speedBoostPerDuplicate : -_speedBoostPerDuplicate;

                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Speed;
                modifier.modifierType = ModifierType.Flat;
                modifier.value = speedBoostPerDuplicate;
            }
        }

        private void ApplyStats()
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int speedBoost = DuplicateCount == 1 ? _speedBoost : -_speedBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.Speed;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = speedBoost;

            playerStats.ApplyModifier(modifier);

        }
    
    }
}
