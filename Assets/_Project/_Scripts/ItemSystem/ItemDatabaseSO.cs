using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "EternalDefenders/ItemSystem/ItemDatabaseSO")]
    [System.Serializable]
    public class ItemDatabaseSO : ScriptableObject
    {
        public List<Item> Items;
    }
}
