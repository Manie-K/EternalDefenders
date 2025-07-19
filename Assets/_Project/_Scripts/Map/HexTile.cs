using System;
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


        void Start()
        {
            TowerController.OnTowerDestroyed += (destroyedTower =>
            {
                if(destroyedTower != null && destroyedTower == Building)
                {
                    ClearBuilding();
                }
            });
        }

        public bool CanBuild() => canBuild && Building is null;
        public void SetBuilding(TowerBase building) => Building = building;
        public void ClearBuilding()
        {
            Building = null;
        }
    }
}