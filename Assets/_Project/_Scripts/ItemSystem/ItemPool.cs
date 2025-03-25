using MG_Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemPool
    {
        private readonly Dictionary<ItemPoolCategory, List<int>> _pool = new();
        private const int MaxRarity = 4;

        public enum ItemPoolCategory
        {
            Regural,
            Shop,
            Boss
        }

        [System.Serializable]
        private class ItemPoolWrapper
        {
            public List<ItemPoolEntry> Pools;
        }

        [System.Serializable]
        private class ItemPoolEntry
        {
            public string Category;
            public List<int> Id;
        }


        public void Initialize()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("Items/item_pools");

            if (jsonFile == null)
            {
                Debug.LogError("Item pool JSON file not found at Items/item_pools");
                return;
            }

            var poolData = JsonUtility.FromJson<ItemPoolWrapper>(jsonFile.text);


            foreach (var entry in poolData.Pools)
            {
                if (System.Enum.TryParse(entry.Category, out ItemPoolCategory poolCategory))
                {
                    _pool[poolCategory] = entry.Id;
                }
                else
                {
                    Debug.LogWarning($"Unknown category '{entry.Category}' in JSON.");
                }
            }
        }

        public int GetRandomItem(ItemPoolCategory category)
        {
            return GetRandomItems(category, 1).First();
        }

        public List<int> GetRandomItems(ItemPoolCategory category, int k, bool replace = false)
        {
            List<int> itemIds = _pool[category];
            List<int> itemWeights = new();

            ItemDatabase itemDatabase = ItemManager.Instance.ItemDictionary;

            int totalWeight = 0;
            foreach (int itemId in itemIds)
            {
                int itemWeight = MaxRarity - itemDatabase.Items[itemId].Item.Rarity + 1;
                itemWeights.Add(itemWeight);
                totalWeight += itemWeight;
            }

            return RandomChoices(itemIds, itemWeights, k, replace);
        }

        public List<T> RandomChoices<T>(List<T> data, List<int> weights, int k, bool replace = false)
        {
            if (data.Count != weights.Count)
            {
                Debug.LogError("The number of data and weights must be equal.");
                return null;
            }

            List<T> selectedItems = new();

            for (int i = 0; i < k; i++)
            {
                if (replace)
                {
                    T selectedItem = SelectItemBasedOnWeight(data, weights);
                    selectedItems.Add(selectedItem);
                }
                else
                {
                    T selectedItem;
                    do
                    {
                        selectedItem = SelectItemBasedOnWeight(data, weights);
                    } while (selectedItems.Contains(selectedItem)); 

                    selectedItems.Add(selectedItem);
                }
            }

            return selectedItems;
        }

        private T SelectItemBasedOnWeight<T>(List<T> data, List<int> weights)
        {
            int totalWeight = 0;
            foreach (var weight in weights)
            {
                totalWeight += weight;
            }

            int randomWeight = UnityEngine.Random.Range(0, totalWeight);
            int currentWeight = 0;

            for (int i = 0; i < data.Count; i++)
            {
                currentWeight += weights[i];
                if (randomWeight < currentWeight)
                {
                    return data[i];
                }
            }

            return data[data.Count - 1];
        }
    }

}
