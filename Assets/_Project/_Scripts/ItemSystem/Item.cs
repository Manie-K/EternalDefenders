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
        private int _ID;

        /// <summary>
        /// Item rarity value between 1-4: higher value means better quality.
        /// </summary>
        private int _rarity;

        /// <summary>
        /// Higher value means higher priority, base value - 5.
        /// </summary>
        private int _priority;

        /// <summary>
        /// Value in seconds
        /// </summary>
        private float _cooldownDuration;
        private float _cooldownRemaining;

        private ItemType _itemType;
        private ItemTarget _itemTarget;
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

        public int ID
        {
            get { return _ID; }
        }

        public int Rarity
        {
            get { return _rarity; }
            protected set { _rarity = value; }
        }

        public int Priority
        {
            get { return _priority; }
            protected set { _priority = value; }
        }

        public float CooldownDuration
        {
            get { return _cooldownDuration; }
        }

        public float CooldownRemaining
        {
            get { return _cooldownRemaining; }
            protected set { _cooldownRemaining = value; }

        }

        public ItemType ItemType
        {
            get { return _itemType; }
        }

        public ItemTarget ItemTarget 
        { 
            get { return _itemTarget; } 
        }

        public List<ItemEffect> ItemEffects
        {
            get { return _itemEffects; }
            protected set { _itemEffects = value; }
        }

        #endregion


        public void Initialize(
            string name, string description, int ID, int rarity, 
            int priority, float cooldownDuration, float cooldownRemaining,
            ItemType itemType, ItemTarget itemTarget)
        {
            this._name = name;
            this._description = description;
            this._ID = ID;
            this._rarity = rarity;
            this._priority = priority;
            this._cooldownDuration = cooldownDuration;
            this._cooldownRemaining = cooldownRemaining;
            this._itemType = itemType;
            this._itemTarget = itemTarget;
            this._itemEffects = new List<ItemEffect>();
        }

        public abstract void UnSubscribe();

        public virtual void Use() { return; }

        public virtual void Update() {  return; }

    }
}
