using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "GuardianAngel", menuName = "EternalDefenders/ItemSystem/Items/GuardianAngel")]
    public class GuardianAngel : Item
    {
        /// <summary>
        /// List of last protected towers with their cooldown time
        /// </summary>
        private List<ProtectedTowerCooldown> _protectedTowers;
        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _protectionCooldown;
        [SerializeField] private int _perctentageHealthRecovery = 100;

        public float ProtectionCooldown
        {
            get { return _protectionCooldown; }
        }

        private class ProtectedTowerCooldown {
            public TowerController Tower { get; set; }
            public float TriggerTime { get; set; }
            public ProtectedTowerCooldown(TowerController tower, float triggerTime)
            {
                Tower = tower;
                TriggerTime = triggerTime;
            }
        }

        public override void Initialize(int id, string name)
        {
            InitializeCommon(
                name: name,
                description: $"Revives fallen tower with its {_perctentageHealthRecovery}% health back",
                id: id,
                rarity: 1,
                priority: 5,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Tower
            );


            ItemEffects.Add(ItemEffect.OnDeath);
            ItemEffects.Add(ItemEffect.PreventsDeath);

            _protectionCooldown = 30;
        }

        public override void Collect()
        {
            if (DuplicateCount == 0)
            {
                _protectedTowers = new List<ProtectedTowerCooldown>();
                ItemManager.Instance.ProtectTower += HandleTowerDestroyed;
            }

            DuplicateCount++;
        }

        public override void Remove()
        {
            if (DuplicateCount == 1) 
            {
                _protectedTowers.Clear();
                ItemManager.Instance.ProtectTower -= HandleTowerDestroyed;
            }

            DuplicateCount--;
        }

        private void UpdateCooldowns()
        {
            float currentTime = Time.time;
            _protectedTowers.RemoveAll(t => t.TriggerTime + _protectionCooldown < currentTime);
        }

        private bool HandleTowerDestroyed(Item item, TowerController towerController)
        {
            UpdateCooldowns();

            if (item == this && !_protectedTowers.Any(ptc => ptc.Tower == towerController))
            {
                
                _protectedTowers.Add(new ProtectedTowerCooldown(towerController, Time.time));
                int healthRecovery = towerController.Stats.GetStat(StatType.MaxHealth) * _perctentageHealthRecovery / 100;


                InstantModifier modifier = ScriptableObject.CreateInstance<InstantModifier>();
                modifier.statType = StatType.Health;
                modifier.modifierType = ModifierType.Flat;
                modifier.limitedDurationTime = 1;
                modifier.value = healthRecovery;

                towerController.Stats.ApplyModifier(modifier);
                
                Debug.Log($"Tower destruction prevented recovered tower {healthRecovery} hp");
                return true;
            }

            Debug.Log("Tower destruction already prevented");
            return false;
        }

    }
}
