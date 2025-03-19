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
        private float _protectionCooldown; 

        private class ProtectedTowerCooldown {
            public TowerController Tower { get; set; }
            public float TriggerTime { get; set; }
            public ProtectedTowerCooldown(TowerController tower, float triggerTime)
            {
                Tower = tower;
                TriggerTime = triggerTime;
            }
        }
    
        public GuardianAngel()
        {
            Initialize(
                name: "Guardian Angel",
                description: "Revives fallen tower",
                ID: 0,
                rarity: 1,
                priority: 5,
                cooldownDuration: 0,
                cooldownRemaining: 0,
                itemType: ItemType.Passive,
                itemTarget: ItemTarget.Tower

            );

            ItemEffects.Add(ItemEffect.OnDeath);
            ItemEffects.Add(ItemEffect.PreventsDeath);

            ItemManager.Instance.ProtectTower += HandleTowerDestroyed;

            _protectedTowers = new List<ProtectedTowerCooldown>();
            _protectionCooldown = 30;
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
                
                Debug.Log("Tower dustruction prevented");
                return true;
            }

            Debug.Log("Tower dustruction allready prevented");
            return false;
        }

        public override void UnSubscribe()
        {
            ItemManager.Instance.ProtectTower -= HandleTowerDestroyed;
        }
    }
}
