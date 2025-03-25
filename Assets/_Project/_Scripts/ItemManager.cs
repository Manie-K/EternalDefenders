using MG_Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemManager : Singleton<ItemManager>
    {
        #region Fields

        private readonly ItemDatabase _itemDictionary = new();
        private readonly List<Item> _equippedItems = new();
        private readonly ItemPool _pool = new();

        #endregion

        #region Properties
        public ItemDatabase ItemDictionary
        {
            get { return _itemDictionary; }
        }
        public List<Item> EquippedItems 
        {
            get { return _equippedItems; }
        }
        public ItemPool Pool { get { return _pool; } }

        #endregion

        #region Events

        public event Func<Item, TowerController, bool> ProtectTower;
        public event Action<Item> OnItemPickUp;
        public event Action<Item> OnItemRemoval;

        #endregion

        private void Start()
        {
            _itemDictionary.Initialize();
            _pool.Initialize();
        }

        private void Update()
        {
            foreach (var item in _equippedItems)
            {
                item.UpdateItem(Time.deltaTime);
            }
        }

        public void UseActiveItem()
        {
            foreach(var item in _equippedItems)
            {
                item.Use();
            }
        }

        public void AddItemByID(int itemId)
        {
            Item item = _itemDictionary.Items[itemId].Item;

            if (item != null)
            {

                if (!_equippedItems.Contains(item))
                {
                    _equippedItems.Add(item);
                }

                item.Collect();

            }
            else
            {
                Debug.LogError($"No item with id:{itemId} exists");
            }
        }

        public void RemoveItemByID(int itemId)
        {
            Item item = _equippedItems[itemId];

            if (item != null)
            {
                OnItemRemoval.Invoke(item);

                item.Remove();

                if (item.DuplicateCount == 0)
                {
                    _equippedItems.RemoveAt(itemId);
                }
            }
            else
            {
                Debug.LogError($"No item with id:{itemId} is equipped");
            }

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

    }
}
