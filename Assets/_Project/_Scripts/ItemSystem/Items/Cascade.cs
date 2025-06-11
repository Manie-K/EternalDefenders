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

        [SerializeField] Dictionary<StatType, int> _boosts;
            /*
        {
            { StatType.Damage, 5 },
            { StatType.Speed, 2 },
            { StatType.MaxHealth, 20 },
        };
            */

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
            KeyValuePair<StatType, int> randomBoost = _boosts.ElementAt(UnityEngine.Random.Range(0, _boosts.Count));

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType =randomBoost.Key;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = randomBoost.Value;

            playerStats.ApplyModifier(modifier);
        }
    }
}
