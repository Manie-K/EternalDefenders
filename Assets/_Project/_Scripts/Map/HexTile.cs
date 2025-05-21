using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField] bool canBuild = true;
        [SerializeField] float buildingHeight = 0f;
        
        public TowerBase Building { get; private set; }
        public float BuildingHeight => buildingHeight;
        
        public bool CanBuild() => canBuild && Building is null;
        public void SetBuilding(TowerBase building) => Building = building;
    }
}