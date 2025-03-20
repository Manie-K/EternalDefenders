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
        [SerializeField] private int _rarity;

        /// <summary>
        /// Higher value means higher priority, base value - 5.
        /// </summary>
        [SerializeField] private int _priority;

        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _cooldownDuration;
        [SerializeField] private float _cooldownRemaining;
        [SerializeField] private float _duplicateCount;

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

        public float DuplicateCount
        {
            get { return _duplicateCount; }
            protected set { _duplicateCount = value; }
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

        public abstract void Initialize(int id, string name);

        protected void InitializeCommon(
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
            this._duplicateCount = 0;
        }
        /// <summary>
        /// Ensure DuplicateCount is updated
        /// </summary>
        public abstract void Collect();

        /// <summary>
        /// Ensure DuplicateCount is updated
        /// </summary>
        public abstract void Remove();

        public virtual void Use() { return; }

        public virtual void Update() {  return; }

    }
}
