using MG_Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemManager : Singleton<ItemManager>
    {
        private List<Item> _items;
        public event Func<Item, TowerController, bool> ProtectTower;

        private void Awake()
        {
            base.Awake();
            _items = new List<Item>
            {
                ScriptableObject.CreateInstance<GuardianAngel>(),
                ScriptableObject.CreateInstance<HealthShot>(),
                ScriptableObject.CreateInstance<UnfathomMalice>()
            };

            Item activeItem = _items.Where(item => item.ItemType == ItemType.Active).ToList().First();

            activeItem.Use();
        }

        public void RemoveItemByID(int itemID)
        {
            Item item = _items[itemID];

            item.UnSubscribe();
            _items.RemoveAt(itemID);
        }

        public void AddItemByID(int itemID)
        {
            Item item = _items[itemID];
        }

        public bool IsTowerProtected(TowerController towerController)
        {
            List<Item> protectiveItems = _items
                .Where(item => item.ItemEffects.Any(effect => effect == ItemEffect.PreventsDeath) && item.ItemTarget == ItemTarget.Tower)
                .OrderByDescending(item => item.Priority)
                .ToList();


            foreach (var item in protectiveItems)
            {
                
                bool isProtected = ProtectTower?.Invoke(item, towerController)??false;
                
                if (isProtected)
                {
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
            foreach (var item in _items)
            {
                item.Update();
            }
        }

    }
}
