using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "TowerBundle", menuName = "EternalDefenders/Building/TowerBundle")]
    public class TowerBundle : ScriptableObject
    {
        [Serializable]
        public class ResourceCost
        {
            public ResourceSO resource;
            public int amount;
        }
        
        public TowerController towerPrefab;
        public Mesh combinedMesh;
        public Sprite icon;
        public List<ResourceCost> cost;
    }
}