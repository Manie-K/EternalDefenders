using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Item", menuName = "EternalDefenders/ItemSystem/Item")]
    public abstract class Item : ScriptableObject
    {
        #region Fields

        [SerializeField] private string _name;
        [SerializeField] private string _description;
        /// <summary>
        /// Unique identifier for every item
        /// </summary>
        [SerializeField] private int _id;

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
        private int _duplicateCount = 0;

        [SerializeField] private bool _unique;
        [SerializeField] private Sprite _icon;
        [SerializeField] public List<TowerBundle.ResourceCost> _cost;

        [SerializeField] private ItemType _itemType;
        [SerializeField] private ItemTarget _itemTarget;
        [SerializeField] private List<ItemEffect> _itemEffects;

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
        public int DuplicateCount
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
        public Sprite Icon
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
