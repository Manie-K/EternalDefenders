using MG_Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class ItemInventory : Singleton<ItemInventory>
    {
        private List<Item> _items;
        private void Awake()
        {
            _items = new List<Item>
            {
                ScriptableObject.CreateInstance<ExampleItem>()
            };
        }

    }
}
