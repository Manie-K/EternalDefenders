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
        [Header("Private fields")]
        [SerializeField] private int _flatDamageBoostPerDuplicate;
        [SerializeField] private int _flatDamageBoost;
        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _damageBurstsInterval;
        [SerializeField] private int _damageBurstValue;
        [SerializeField] private int _damageBurstDuration;

        private float _triggerTime;

        public float TriggerTime
        {
            get { return _triggerTime; }
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
                modifier.persistAfterFinish = true;
                modifier.limitedDurationTime = 0.01f;
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
