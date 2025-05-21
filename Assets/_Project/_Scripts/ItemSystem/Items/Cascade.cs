using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Cascade", menuName = "EternalDefenders/ItemSystem/Items/Cascade")]
    public class Cascade : Item
    {

        private readonly Dictionary<StatType, int> boosts = new()
        {
            { StatType.Damage, 5 },
            { StatType.Speed, 2 },
            { StatType.MaxHealth, 20 },
        };

        public override void Initialize(int id, string name)
        {
            List<TowerBundle.ResourceCost> cost = new() {
                new ResourceCost
                {
                    resource = new(),
                    amount = 600
                },
                new ResourceCost
                {
                    resource = new(),
                    amount = 600
                }
            };

            InitializeCommon(
                name: name,
                description: $"Adds permament random flat buff to player on item pick up",
                id: id,
                icon: null,
                rarity: Rarity.Legendary,
                cost: cost,
                unique: true,
                priority: 5,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Player
            );
        }


        public override void Collect()
        {
            if (DuplicateCount == 0)
            {
                DuplicateCount++;
                ItemManager.Instance.OnItemPickUp += ApplyRandomStat;
            }

        }

        public override void Remove()
        {
            if (DuplicateCount == 1)
            {
                DuplicateCount++;
                ItemManager.Instance.OnItemPickUp -= ApplyRandomStat;
            }

        }

        private void ApplyRandomStat(Item item)
        {
            Stats playerStats = PlayerController.Instance.Stats;
            KeyValuePair<StatType, int> randomBoost = boosts.ElementAt(UnityEngine.Random.Range(0, boosts.Count));

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType =randomBoost.Key;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = randomBoost.Value;

            playerStats.ApplyModifier(modifier);
        }
    }
}
