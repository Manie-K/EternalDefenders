using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField] bool canBuild = true;
        
        public TowerController Building { get; private set; }
        
        
        public bool CanBuild() => canBuild && Building is null;
        public void SetBuilding(TowerController building) => Building = building;
    }
}