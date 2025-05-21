using Codice.Client.Common.GameUI;
using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Lard", menuName = "EternalDefenders/ItemSystem/Items/Lard")]
    public class Lard : Item
    {
        [SerializeField] private int _maxHealthBoost = 50;

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
                description: $"Gives {_maxHealthBoost} max health",
                id: id,
                icon: null,
                rarity: Rarity.Common,
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

        }

        public override void Remove()
        {
            DuplicateCount--;
            
            ApplyStats(false);
           
        }


        private void ApplyStats(bool wasDuplicateCountRaised)
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int maxHealthBoost = wasDuplicateCountRaised ? _maxHealthBoost : -_maxHealthBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.MaxHealth;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = maxHealthBoost;

            playerStats.ApplyModifier(modifier);

        }
    }
}
