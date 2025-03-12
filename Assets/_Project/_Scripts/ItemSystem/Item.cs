using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Item", menuName = "EternalDefenders/ItemSystem/Item")]
    public abstract class Item : ScriptableObject
    {

        #region Fields
        
        private string _name;
        private string _description;
        /// <summary>
        /// Unique identifier for every item
        /// </summary>
        private int _itemID;

        private ItemType _itemType;

        /// <summary>
        /// Item rarity value between 1-4: higher value means better quality.
        /// </summary>
        private int _itemRarity;

        /// <summary>
        /// Higher value means higher priority, base value - 5.
        /// </summary>
        private int _itemPriority;

        private List<ItemEffect> _itemEffects;

        #endregion

        #region Properties

        public string Name
        {
            get { return _name; }
            protected set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }

        public int ItemID
        {
            get { return _itemID; }
        }

        public ItemType ItemType
        {
            get { return _itemType; }
        }

        public int ItemRarity
        {
            get { return _itemRarity; }
            protected set { _itemRarity = value; }
        }

        public int ItemPriority
        {
            get { return _itemPriority; }
            protected set { _itemPriority = value; }
        }

        public List<ItemEffect> ItemEffects
        {
            get { return _itemEffects; }
            protected set { _itemEffects = value; }
        }

        #endregion


        public void Initialize(
            string name, string description, int itemID, ItemType itemType,
            int itemRarity, int ItemPriority = 5)
        {
            this._name = name;
            this._description = description;
            this._itemID = itemID;
            this._itemType = itemType;
            this._itemRarity = itemRarity;
            this._itemPriority = ItemPriority;
            this._itemEffects = new List<ItemEffect>();
        }
    }
}
