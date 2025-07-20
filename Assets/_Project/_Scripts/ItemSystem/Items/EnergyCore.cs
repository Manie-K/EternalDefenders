using UnityEngine;

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
            int speedBoost = wasDuplicateCountRaised
                ? (DuplicateCount == 1 ? _speedBoost : _speedBoostPerDuplicate)
                : (DuplicateCount == 0 ? -_speedBoost : -_speedBoostPerDuplicate);

            InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
            modifier.statType = StatType.Speed;
            modifier.modifierType = ModifierType.Flat;
            modifier.value = speedBoost;
            modifier.persistAfterFinish = true;
            modifier.limitedDurationTime = 0.01f;

            playerStats.ApplyModifier(modifier);

        }
    
    }
}
