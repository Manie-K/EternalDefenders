using Codice.Client.Common.GameUI;
using Mono.Cecil;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;
using static EternalDefenders.TowerBundle;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "UnfathomMalice", menuName = "EternalDefenders/ItemSystem/Items/UnfathomMalice")]
    public class UnfathomMalice : Item
    {
        [SerializeField] private readonly int _flatDamageBoostPerDuplicate = 1;
        [SerializeField] private readonly int _flatDamageBoost = 5;
        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private readonly float _damageBurstsInterval = 10;
        [SerializeField] private readonly int _damageBurstValue = 10;
        [SerializeField] private readonly int _damageBurstDuration = 5;

        private float _triggerTime;

        public float TriggerTime
        {
            get { return _triggerTime; }
        }

        public override void Initialize(int id, string name)
        {
            List<TowerBundle.ResourceCost> cost = new() {
                new ResourceCost
                {
                    resource = new(),
                    amount = 100
                },
                new ResourceCost
                {
                    resource = new(),
                    amount = 400
                }
            };

            InitializeCommon(
                name: name,
                description: $"Gives dame bursts every {_damageBurstsInterval} seconds",
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
                _triggerTime = Time.time;
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
                int flatDamageBoostPerDuplicate = wasDuplicateCountRaised ? _flatDamageBoostPerDuplicate : -_flatDamageBoostPerDuplicate;

                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Damage;
                modifier.modifierType = ModifierType.Flat;
                modifier.value = flatDamageBoostPerDuplicate;
            }
        }

        private void ApplyStats()
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int damageBoost = DuplicateCount == 1 ? _flatDamageBoost : -_flatDamageBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.Damage;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = damageBoost;

            playerStats.ApplyModifier(modifier);

        }

        public override void UpdateItem(float dt)
        {
            if (Time.time > _triggerTime + _damageBurstsInterval)
            {
                Debug.Log($"{Name}: applied {_damageBurstValue} damage burst");

                Stats playerStats = PlayerController.Instance.Stats;

                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Damage;
                modifier.modifierType = ModifierType.Flat;
                modifier.limitedDurationTime = _damageBurstDuration;
                modifier.value = _damageBurstValue;

                playerStats.ApplyModifier(modifier);

                _triggerTime += _damageBurstsInterval;
            }
        }

    }
}
