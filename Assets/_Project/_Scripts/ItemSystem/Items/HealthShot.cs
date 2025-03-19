using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "HealthShot", menuName = "EternalDefenders/ItemSystem/Items/HealthShot")]
    public class HealthShot : Item
    {
        private float _healthPercentageRegen;
        /// <summary>
        /// Value in seconds
        /// </summary>
        private float _healthRegenDuration;
        private float _healthUpdateInterval;
        private float _triggerTime;
        private float _elapsedTimeSinceLastInterval;
        private bool _isActive;


        public HealthShot()
        {
            Initialize(
                name: "HealthShot",
                description: "Quickly heals player over short period of time",
                ID: 1,
                rarity: 1,
                priority: 5,
                cooldownDuration: 60,
                cooldownRemaining: 0,
                itemType: ItemType.Active,
                itemTarget: ItemTarget.None
            );

            _healthPercentageRegen = 1.5f;

            // Ensure health regen duration is divisible by the update interval for consistent updates
            _healthUpdateInterval = 1;
            _healthRegenDuration = 20;
            _isActive = false;

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

                    Debug.Log($"Regened player by {(int)healthRegenValue}");
                    // Round down value
                    playerStats.ChangeStat(StatType.Health, (int)healthRegenValue);
                  
                    _elapsedTimeSinceLastInterval -= _healthUpdateInterval;

                }


                if (Time.time > _triggerTime + _healthRegenDuration)
                {
                    _isActive = false;
                    CooldownRemaining = CooldownDuration;
                }
            }

        }

        public override void UnSubscribe()
        {
            return;
        }
    }
}
