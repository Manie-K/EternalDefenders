using Mono.Cecil;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor.Graphs;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "UnfathomMalice", menuName = "EternalDefenders/ItemSystem/Items/UnfathomMalice")]
    public class UnfathomMalice : Item
    {
        [SerializeField] private int _flatDamageBoost = 5;
        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _damageBurstsInterval = 10;
        [SerializeField] private int _damageBurstValue = 10;
        [SerializeField] private int _damageBurstDuration = 5;

        private float _triggerTime;

        public float TriggerTime
        {
            get { return _triggerTime; }
        }

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: $"Gives dame bursts every {_damageBurstsInterval} seconds",
                id: id,
                rarity: 2,
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
                _triggerTime = Time.time;
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

            int damageBoost = DuplicateCount == 1 ? _flatDamageBoost : -_flatDamageBoost;

            InstantModifier modifier = new InstantModifier()
            {
                statType = StatType.Damage,
                modifierType = ModifierType.Flat,
                value = damageBoost
            };

            playerStats.ApplyModifier(modifier);

        }

        public override void Update()
        {
            if (Time.time > _triggerTime + _damageBurstsInterval)
            {
                Debug.Log($"{Name}: applied {_damageBurstValue} damage burst");

                Stats playerStats = PlayerController.Instance.Stats;

                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Damage;
                modifier.modifierType = ModifierType.Flat;
                modifier.limitedDurationTime = _damageBurstDuration;
                modifier.value = _damageBurstValue;

                playerStats.ApplyModifier(modifier);

                _triggerTime += _damageBurstsInterval;
            }
        }

    }
}
