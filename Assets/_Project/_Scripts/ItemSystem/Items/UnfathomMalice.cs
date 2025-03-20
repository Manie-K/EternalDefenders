using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "UnfathomMalice", menuName = "EternalDefenders/ItemSystem/Items/UnfathomMalice")]
    public class UnfathomMalice : Item
    {
        [SerializeField] private int _lastDamageBoost;
        [SerializeField] private int _minFlatDamageBoost = 5;
        [SerializeField] private int _maxFlatDamageBoost = 20;
        /// <summary>
        /// Amount of health in percentage player needs to be under so the multipier applies
        /// </summary>
        [SerializeField] private float _damageMultiplierThreshold = 0.4f;
        [SerializeField] private int _damageMultiplierPercentage = 50;

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: "Gives powerful damage boost the lower player health is",
                ID: id,
                rarity: 2,
                priority: 5,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Player
            );

            _lastDamageBoost = 0;
        }

        public override void Collect()
        {
            DuplicateCount++;
        }

        public override void Remove()
        {
            DuplicateCount--;
        }

        public override void Update()
        {
            Stats playerStats = PlayerController.Instance.Stats;

            float remainingHealthRatio = 1 - (float)playerStats.GetStat(StatType.Health) / playerStats.GetStat(StatType.MaxHealth);
            int currentDamageBoost = Mathf.RoundToInt(_minFlatDamageBoost + (_maxFlatDamageBoost - _minFlatDamageBoost) * remainingHealthRatio - _lastDamageBoost);

            playerStats.ChangeStat(StatType.Damage, currentDamageBoost);


            InstantModifier modifer = new InstantModifier()
            {
                statType = StatType.Damage,
                modifierType = ModifierType.Flat,
                value = currentDamageBoost
            };


            if (remainingHealthRatio > 1 - _damageMultiplierThreshold)
            {
                // ToDo stat multiplication
            }

            _lastDamageBoost = currentDamageBoost;
        }

    }
}
