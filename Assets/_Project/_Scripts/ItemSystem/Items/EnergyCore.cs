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

        [SerializeField] private int _speedBoost;
        [SerializeField] private int _speedBoostPerDuplicate;

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
                Stats playerStats = PlayerController.Instance.Stats;

                int speedBoostPerDuplicate = wasDuplicateCountRaised ? _speedBoostPerDuplicate : -_speedBoostPerDuplicate;

                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Speed;
                modifier.modifierType = ModifierType.Flat;
                modifier.value = speedBoostPerDuplicate;

                playerStats.ApplyModifier(modifier);
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
