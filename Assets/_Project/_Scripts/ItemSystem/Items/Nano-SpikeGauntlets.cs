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
        [SerializeField] private int _flatDamageBoost;

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
            modifier.persistAfterFinish = true;
            modifier.limitedDurationTime = 0.01f;

            playerStats.ApplyModifier(modifier);

        }
    }
}
