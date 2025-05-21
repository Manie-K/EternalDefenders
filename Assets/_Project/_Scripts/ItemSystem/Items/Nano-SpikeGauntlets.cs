using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Nano-SpikeGauntlets", menuName = "EternalDefenders/ItemSystem/Items/Nano-SpikeGauntlets")]
    public class NanoSpikeGauntlets : Item
    {

        [SerializeField] private readonly int _flatDamageBoost = 10;

        public override void Initialize(int id, string name)
        {
            List<TowerBundle.ResourceCost> cost = new() {
                new ResourceCost
                {
                    resource = new(),
                    amount = 200
                },
                new ResourceCost
                {
                    resource = new(),
                    amount = 200
                }
            };

            InitializeCommon(
                name: name,
                description: $"Adds {_flatDamageBoost} flat damage buff to player",
                id: id,
                icon: null,
                rarity: Rarity.Common,
                cost: cost,
                unique: false,
                priority: 5,
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

        }

        public override void Remove()
        {
            DuplicateCount--;
            ApplyStats(false);
        }

        private void ApplyStats(bool wasDuplicateCountRaised)
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int damageBoost = wasDuplicateCountRaised ? _flatDamageBoost : -_flatDamageBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.Damage;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = damageBoost;

            playerStats.ApplyModifier(modifier);

        }
    }
}
