using Codice.CM.Common;
using MG_Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemDatabase
    {
        private readonly Dictionary<int, ItemInfo> _items = new();

        public Dictionary<int, ItemInfo> Items => _items;


        [System.Serializable]
        private class ItemDatabaseWrapper
        {
            public List<ItemData> itemDatas;
        }

        [System.Serializable]
        public class ItemData
        {
            public int Id;
            public string Name;
            public string Type;
        }

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

        /// <summary>
        /// Reads item data from json file and initializes items
        /// </summary>
        public void Initialize()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Items/item_database");

            if (jsonFile == null)
            {
                Debug.LogError($"Item database JSON file not found at Items/item_database");
                return;
            }

            var itemsData = JsonUtility.FromJson<ItemDatabaseWrapper>(jsonFile.text);


            foreach (var itemData in itemsData.itemDatas)
            {
                Item itemInstance = CreateItemByType(itemData.Type);

                if (itemInstance == null)
                {
                    Debug.LogWarning($"Item type {itemData.Type} not found.");
                    continue;
                }

                ItemInfo itemInfo = new ItemInfo(itemData.Id, itemData.Name, itemInstance);

                // Important initializes items
                itemInstance.Initialize(itemData.Id, itemData.Name);

                _items.Add(itemData.Id, itemInfo);
            }

            Debug.Log("Item database loaded. Total items: " + _items.Count);
        }

        private Item CreateItemByType(string typeName)
        {
            var type = Type.GetType("EternalDefenders." + typeName);

            if (type == null)
            {
                Debug.LogError($"Type '{typeName}' not found. Make sure the namespace is included if needed.");
                return null;
            }

            var item = ScriptableObject.CreateInstance(type) as Item;

            if (item == null)
            {
                Debug.LogError($"Type '{typeName}' is not a valid Item.");
            }

            return item;
        }

    }

}
