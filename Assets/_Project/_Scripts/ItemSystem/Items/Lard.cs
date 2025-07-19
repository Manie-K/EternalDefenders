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
        [SerializeField] private int _maxHealthBoost;

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

            InstantModifier maxHealthModifier = ScriptableObject.CreateInstance<InstantModifier>();
            maxHealthModifier.statType = StatType.MaxHealth;
            maxHealthModifier.modifierType = ModifierType.Flat;
            maxHealthModifier.value = maxHealthBoost;
            maxHealthModifier.persistAfterFinish = true;
            maxHealthModifier.limitedDurationTime = 0.01f;

            InstantModifier healthModifier = ScriptableObject.CreateInstance<InstantModifier>();
            healthModifier.statType = StatType.Health;
            healthModifier.modifierType = ModifierType.Flat;
            healthModifier.value = maxHealthBoost;
            healthModifier.persistAfterFinish = true;
            healthModifier.limitedDurationTime = 0.01f;

            playerStats.ApplyModifier(maxHealthModifier);
            playerStats.ApplyModifier(healthModifier);

        }
    }
}
