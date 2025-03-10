using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class PlayerResourceInventory : Singleton<PlayerResourceInventory>
    {
        readonly Dictionary<ResourceSO, int> _resources = new();
     
        public int GetResourceAmount(ResourceSO resourceType)
        {
            return _resources.ContainsKey(resourceType) ? _resources[resourceType] : 0;
        }
        
        public void AddResource(ResourceSO resourceType, int amount)
        {
            if(amount <= 0) return;
            if (!_resources.TryAdd(resourceType, amount))
            {
                _resources[resourceType] += amount;
            }
        }
        
        public int RemoveResource(ResourceSO resourceType, int amount)
        {
            if(amount <= 0) return 0;
            if(!_resources.ContainsKey(resourceType))
                return 0;
            
            
            int amountToRemove = Mathf.Min(amount, _resources[resourceType]);
            _resources[resourceType] -= amountToRemove;
            if (_resources[resourceType] <= 0)
            {
                _resources.Remove(resourceType);
            }
            return amountToRemove;
        }
        
        public bool HasEnoughOfResource(ResourceSO resourceType, int amount)
        {
            if(amount <= 0) return true;
            
            return _resources.ContainsKey(resourceType) && _resources[resourceType] >= amount;
        }
    }
}