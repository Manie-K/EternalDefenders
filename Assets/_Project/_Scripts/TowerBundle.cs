using UnityEngine;

namespace EternalDefenders
{
    [CreateAssetMenu(fileName = "TowerBundle", menuName = "EternalDefenders/Building/TowerBundle")]
    public class TowerBundle : ScriptableObject
    {
        public TowerController towerPrefab;
        public Sprite icon;
        
        //TODO: Handle building cost
        public int cost;
    }
}