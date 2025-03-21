using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "EnergyCore", menuName = "EternalDefenders/ItemSystem/Items/EnergyCore")]
    public class EnergyCore : Item
    {

        [SerializeField] private int _speedBoost = 5;
        [SerializeField] private int _speedBoostPerDuplicate = 1;

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: "Gives boost to player attack and movement speed",
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

            int speedBoost = DuplicateCount == 1 ? _speedBoost : -_speedBoost;

            InstantModifier modifier = new InstantModifier()
            {
                statType = StatType.Speed,
                modifierType = ModifierType.Flat,
                value = speedBoost
            };

            playerStats.ApplyModifier(modifier);

        }
    
    }
}
