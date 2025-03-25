using UnityEngine;

namespace EternalDefenders
{
    public class Overclock : Item
    {
        [SerializeField] private readonly int _attackSpeedBoost = -2;
        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: $"Boosts attack speed of a player",
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
            DuplicateCount++;

            if (DuplicateCount == 1)
            {
                ApplyStats();
            }

        }

        public override void Remove()
        {
            DuplicateCount--;

            if (DuplicateCount == 0)
            {
                ApplyStats();
            }
        }

        private void ApplyStats()
        {
            Stats playerStats = PlayerController.Instance.Stats;

            int attackSpeedBoost = DuplicateCount == 1 ? _attackSpeedBoost : -_attackSpeedBoost;

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();

            modifier.statType = StatType.Cooldown;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = attackSpeedBoost;

            playerStats.ApplyModifier(modifier);

        }
    }
}
