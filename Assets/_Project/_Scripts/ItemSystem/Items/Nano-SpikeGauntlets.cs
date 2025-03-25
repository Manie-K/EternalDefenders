using Mono.Cecil;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Nano-SpikeGauntlets", menuName = "EternalDefenders/ItemSystem/Items/Nano-SpikeGauntlets")]
    public class NanoSpikeGauntlets : Item
    {

        [SerializeField] private readonly int _flatDamageBoost = 20;

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: $"Adds {_flatDamageBoost} flat damage buff to player",
                id: id,
                rarity: 1,
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
                ApplyStats();
            }
            DuplicateCount++;
        }

        public override void Remove()
        {
            if (DuplicateCount == 1)
            {
                ApplyStats();
            }
            DuplicateCount--;
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
    }
}
