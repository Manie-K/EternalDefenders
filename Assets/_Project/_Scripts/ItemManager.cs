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
        private ItemDatabase _itemDictionary = new ItemDatabase();
        private List<Item> _equippedItems = new List<Item>();

        public event Func<Item, TowerController, bool> ProtectTower;

        public ItemDatabase ItemDitionary { get; private set; }
        public List<Item> EquippedItems { get; private set; }

        private void Awake()
        {
            base.Awake();

            _itemDictionary.FillData();

            AddItemByID(0);
            AddItemByID(1);
            AddItemByID(2);

            Item activeItem = _equippedItems.Where(item => item.ItemType == ItemType.Active).ToList().First();

            activeItem.Use();
        }

        public void RemoveItemByID(int itemID)
        {
            Item item = _equippedItems[itemID];

            item.Remove();

            if (item.DuplicateCount == 0)
            {
                _equippedItems.RemoveAt(itemID);
            }
        }

        public void AddItemByID(int itemID)
        {
            Item item = _itemDictionary.Items[itemID].Item;

            if (!_equippedItems.Contains(item))
            {
                _equippedItems.Add(item);
            }

            item.Collect();
        }

        public bool IsTowerProtected(TowerController towerController)
        {
            List<Item> protectiveItems = _equippedItems
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
            foreach (var item in _equippedItems)
            {
                item.Update();
            }
        }

    }
}
