using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ExampleItem", menuName = "EternalDefenders/ItemSystem/Items/ExampleItem")]
    public class ExampleItem : Item
    {
        /// <summary>
        /// List of last protected towers with their cooldown time
        /// </summary>
        private List<ProtectedTowerCooldown> _protectedTowers;
        /// <summary>
        /// In seconds
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
    
        public ExampleItem()
        {
            Initialize(
                name: "Name",
                description: "Description",
                itemID: 0,
                itemType: ItemType.Passive,
                itemRarity: 1
            );

            ItemEffects.Add(ItemEffect.OnDeath);
            ItemEffects.Add(ItemEffect.PreventsDeath);

            ItemManager.ProtectTower += HandleTowerDestroyed;

            _protectedTowers = new List<ProtectedTowerCooldown>();
            _protectionCooldown = 30;
        }

        private void UpdateCooldowns()
        {
            float currentTime = Time.time;
            _protectedTowers.RemoveAll(t => t.TriggerTime + _protectionCooldown < currentTime);
        }
        private void HandleTowerDestroyed(Item item, TowerController towerController, Wrapper<bool> isProtected)
        {
            UpdateCooldowns();

            if (item == this && !_protectedTowers.Any(ptc => ptc.Tower == towerController))
            {
                towerController.Stats.SetStat(StatType.Health, 1000);
                towerController.Stats.SetStat(StatType.Cooldown, 1);
                
                _protectedTowers.Add(new ProtectedTowerCooldown(towerController, Time.time));
                isProtected.Value = true;
                
                Debug.Log("Tower dustruction prevented");
                return;
            }

            isProtected.Value = false;
            Debug.Log("Tower dustruction allready prevented");
        }

    }
}
