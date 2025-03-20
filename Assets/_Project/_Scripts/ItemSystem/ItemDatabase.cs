using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemDatabase
    {
        private readonly Dictionary<int, ItemInfo> _items;

        public Dictionary<int, ItemInfo> Items => _items;

        public class ItemInfo
        {
            public int Id;
            public string Name;
            public Item Item;

            public ItemInfo(int id, string name, Item item)
            {
                Id = id;
                Name = name;
                Item = item;
            }
        }
        public ItemDatabase()
        {
            _items = new Dictionary<int, ItemInfo>();
        }

        public void FillData()
        {
            if (_items.Count > 0)
            {
                return;
            }

            Register(0, "Guardian Angel", ScriptableObject.CreateInstance<GuardianAngel>());
            Register(1, "Health Shot", ScriptableObject.CreateInstance<HealthShot>());
            Register(2, "Unfathom Malice", ScriptableObject.CreateInstance<UnfathomMalice>());
        }


        private void Register(int id, string name, Item item)
        {
            _items[id] = new ItemInfo(id, name, item);
            item.Initialize(id, name);
        }
    }

}
