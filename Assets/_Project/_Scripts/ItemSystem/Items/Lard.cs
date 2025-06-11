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

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.MaxHealth;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = maxHealthBoost;

            playerStats.ApplyModifier(modifier);

        }
    }
}
