using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "HealthShot", menuName = "EternalDefenders/ItemSystem/Items/HealthShot")]
    public class HealthShot : Item
    {
        [SerializeField] private float _healthPercentageRegen = 2.0f;
        /// <summary>
        /// Updates in a second
        /// </summary>
        [SerializeField] private int _regenerationTickRate = 1;
        [SerializeField] private int _regenerationDuration = 10;
        private bool _isActive;

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: "Quickly heals player over short period of time",
                id: id,
                rarity: 1,
                priority: 5,
                cooldownDuration: 60,
                cooldownRemaining: 0,
                itemType: ItemType.Active,
                itemTarget: ItemTarget.None
            );

            _isActive = false;
        }

        public override void Collect()
        {
            DuplicateCount++;
        }

        public override void Remove()
        {
            _isActive = false;
            CooldownRemaining = 0;

            DuplicateCount--;
        }

        public override void Use()
        {
            if (CooldownRemaining == 0)
            {
                CooldownRemaining = CooldownDuration;
                Stats playerStats = PlayerController.Instance.Stats;

                float healthRegenValue = playerStats.GetStat(StatType.Health) * _healthPercentageRegen;
                int healthRegenPerTick = Mathf.RoundToInt(healthRegenValue / _regenerationDuration);

                OverTimeModifier modifier = ScriptableObject.CreateInstance<OverTimeModifier>();
                modifier.statType = StatType.Health;
                modifier.tickRate = _regenerationTickRate;
                modifier.tickValue = healthRegenPerTick;

                playerStats.ApplyModifier(modifier);
            }
        }

        public override void Update(float dt) 
        {
            CooldownRemaining = CooldownRemaining - dt > 0 ? CooldownRemaining - dt : 0;
        }
    }
}
