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
        private int _id;

        /// <summary>
        /// Item rarity value between 1-4: higher value means better quality.
        /// </summary>
        [SerializeField] private Rarity _rarity;

        /// <summary>
        /// Higher value means higher priority, base value - 5.
        /// </summary>
        [SerializeField] private int _priority;

        /// <summary>
        /// Value in seconds
        /// </summary>
        [SerializeField] private float _cooldownDuration;
        private float _cooldownRemaining;
        private float _duplicateCount;


        [SerializeField] private bool _unique;
        private Sprite _icon;
        [SerializeField] public List<TowerBundle.ResourceCost> _cost;

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

        public int Id
        {
            get { return _id; }
        }

        public Rarity Rarity
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

        public bool Unique
        {
            get { return _unique; }
        }
        public Sprite icon
        {
            get { return _icon; }
            protected set { _icon = value; }
        }

        public List<TowerBundle.ResourceCost> Cost
        {
            get { return _cost; }
            protected set { _cost = value; }
        }

        #endregion

        public abstract void Initialize(int id, string name);

        protected void InitializeCommon(
            string name, string description, int id, Sprite icon, Rarity rarity, 
            List<TowerBundle.ResourceCost> cost, int priority, bool unique, 
            float cooldownDuration, float cooldownRemaining,
            ItemType itemType, ItemTarget itemTarget)
        {
            this._name = name;
            this._description = description;
            this._id = id;
            this._icon = icon;
            this._rarity = rarity;
            this._cost = cost;
            this._priority = priority;
            this._unique = unique;
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

        public virtual void UpdateItem(float dt) {  return; }

    }
}
