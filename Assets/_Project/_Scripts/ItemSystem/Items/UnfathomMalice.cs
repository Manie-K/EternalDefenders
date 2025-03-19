using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "UnfathomMalice", menuName = "EternalDefenders/ItemSystem/Items/UnfathomMalice")]
    public class UnfathomMalice : Item
    {
        private int _lastDamageBoost;
        private int _maxFlatDamageBoost;
        /// <summary>
        /// Amount of health in percentage player needs to be under so the multipier applies
        /// </summary>
        private float _damageMultiplierThreshold;
        private float _damageMultiplier;
         

        public UnfathomMalice()
        {
            Initialize(
                name: "Unfathom Malice",
                description: "Gives powerful damage boost the lower player health is",
                ID: 2,
                rarity: 2,
                priority: 5,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Player
            );

            _lastDamageBoost = 0;
            _maxFlatDamageBoost = 10;
            _damageMultiplier = 2.0f;
            _damageMultiplierThreshold = 0.4f;
        }

        public override void Update()
        {
            Stats playerStats = PlayerController.Instance.Stats;

            float remainingHealthRatio = 1 - (float)playerStats.GetStat(StatType.Health) / playerStats.GetStat(StatType.MaxHealth);
            int currentDamageBoost = (int)(_maxFlatDamageBoost * remainingHealthRatio - _lastDamageBoost);

            playerStats.ChangeStat(StatType.Damage, currentDamageBoost);

            if (remainingHealthRatio > 1 - _damageMultiplierThreshold)
            {
                // ToDo stat multiplication
            }

            _lastDamageBoost = currentDamageBoost;
        }

        public override void UnSubscribe()
        {
            return;
        }
    }
}
