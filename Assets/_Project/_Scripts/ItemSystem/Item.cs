using Codice.Client.BaseCommands.Merge.Xml;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "Item", menuName = "EternalDefenders/ItemSystem/Item")]
    public abstract class Item : ScriptableObject
    {
        [SerializeField] protected string ItemName;
        [SerializeField] protected string ItemDescription;
        [SerializeField] protected int ItemID;
        [SerializeField] protected ItemType ItemType;
        [SerializeField] protected int ItemRarity; // value between 1 - 4 higher better quality item


        public void Initialize(string itemName, string itemDescription, int itemID, ItemType itemType, int itemRarity)
        {
            this.ItemName = itemName;
            this.ItemDescription = itemDescription;
            this.ItemID = itemID;
            this.ItemType = itemType;
            this.ItemRarity = itemRarity;
        }
    }
}
