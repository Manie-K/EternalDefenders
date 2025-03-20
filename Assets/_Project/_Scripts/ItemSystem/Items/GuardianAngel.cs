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
                description: "Revives fallen tower",
                ID: id,
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
                
                Debug.Log("Tower dustruction prevented");
                return true;
            }

            Debug.Log("Tower dustruction allready prevented");
            return false;
        }

    }
}
