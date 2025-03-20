using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "HealthShot", menuName = "EternalDefenders/ItemSystem/Items/HealthShot")]
    public class HealthShot : Item
    {
        [SerializeField] private float _healthPercentageRegen = 1.5f;
        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _healthRegenDuration = 20;
        [SerializeField] private float _healthUpdateInterval = 1;
        [SerializeField] private float _triggerTime;
        [SerializeField] private float _elapsedTimeSinceLastInterval;
        private bool _isActive;

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: "Quickly heals player over short period of time",
                ID: id,
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
            _isActive = true;
            _triggerTime = Time.time;
            _elapsedTimeSinceLastInterval = 0;
        }

        public override void Update() 
        {
            if (CooldownRemaining > 0f) {
                CooldownRemaining = Mathf.Max(0f, CooldownRemaining - Time.deltaTime);
            }

            if (_isActive)
            {
                _elapsedTimeSinceLastInterval += Time.deltaTime;

                Stats playerStats = PlayerController.Instance.Stats;

                while (_elapsedTimeSinceLastInterval >= _healthUpdateInterval) {
                    float healthRegenValue = playerStats.GetStat(StatType.MaxHealth) * _healthPercentageRegen / (_healthRegenDuration /  _healthUpdateInterval);

                    Debug.Log($"Regened player by {Mathf.RoundToInt(healthRegenValue)}");

                    InstantModifier modifier = new InstantModifier()
                    {
                        statType = StatType.Health,
                        modifierType = ModifierType.Flat,
                        value = Mathf.RoundToInt(healthRegenValue)
                    };

                    playerStats.ApplyModifier(modifier);
                  
                    _elapsedTimeSinceLastInterval -= _healthUpdateInterval;

                }


                if (Time.time > _triggerTime + _healthRegenDuration)
                {
                    _isActive = false;
                    CooldownRemaining = CooldownDuration;
                }
            }

        }
    }
}
