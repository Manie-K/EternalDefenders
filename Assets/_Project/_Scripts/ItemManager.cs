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
        public static event Action<Item, TowerController, Wrapper<bool>> ProtectTower;

        private void Awake()
        {
            base.Awake();
            _items = new List<Item>
            {
                ScriptableObject.CreateInstance<ExampleItem>()
            };

        }

        public bool IsTowerProtected(TowerController towerController)
        {
            List<Item> protectiveItems = _items
                .Where(item => item.ItemEffects.Any(effect => effect == ItemEffect.PreventsDeath))
                .OrderByDescending(item => item.ItemRarity)
                .ToList();

            Wrapper<bool> isProtected = new Wrapper<bool>(false);
            foreach (var item in protectiveItems)
            {
                ProtectTower.Invoke(item, towerController, isProtected);
                
                if (isProtected.Value == true)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
