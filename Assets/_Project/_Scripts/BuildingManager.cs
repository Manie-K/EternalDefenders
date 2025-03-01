using System;
using System.Collections.Generic;
using EternalDefenders.Helpers;
using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class BuildingManager : Singleton<BuildingManager>
    {
        [SerializeField] Transform towersParent;
        [SerializeField] Color ghostValidColor;
        [SerializeField] Color ghostInvalidColor;
        public event Action OnBuildingModeEnter;
        public event Action OnBuildingModeExit;
        
        MeshFilter _ghostFilter;
        MeshRenderer _ghostRenderer;
        GameObject _ghost;
        TowerController _selectedTower;
        bool _isEnabled = false;

        void Start()
        {
            _ghost = transform.GetChild(0).gameObject;
            if(_ghost == null)
            {
                Debug.LogError("Ghost object not found!");
            }
            
            _ghostFilter = _ghost.GetComponent<MeshFilter>();
            _ghostRenderer = _ghost.GetComponent<MeshRenderer>();
            if(_ghostRenderer == null || _ghostFilter == null)
            {
                Debug.LogError("Ghost mesh not found!");
            }
            
            OnBuildingModeEnter += () =>
            {
                _isEnabled = true;
                _ghost.SetActive(true);
            };
            OnBuildingModeExit += () =>
            {
                _isEnabled = false;
                _selectedTower = null;
                _ghostFilter.mesh = null;
                _ghost.SetActive(false);
            };

            BuildingConstructionManger.Instance.OnBuildingSelected += OnBuildingSelected_Delegate;
            
            OnBuildingModeExit?.Invoke();
        }

        void OnDisable()
        {
            BuildingConstructionManger.Instance.OnBuildingSelected -= OnBuildingSelected_Delegate;
        }
        void Update()
        {
            //TODO: Refactor to use input system, maybe change event origin.
            if(Input.GetKeyDown(KeyCode.B))
            {
                if(_isEnabled) OnBuildingModeExit?.Invoke();
                else OnBuildingModeEnter?.Invoke();
            }   
                
            if(!_isEnabled || _selectedTower == null) return;

            //Optimization: Might be a bit expensive
            HexTile tile = HexMapController.Instance.GetHexTileFromWorldPosition(
                   CameraController.Instance.GetWorldMousePosition()
               );
            
            _ghostRenderer.material.color = tile.CanBuild() ? ghostValidColor : ghostInvalidColor;
            _ghost.transform.position = tile.transform.position;
            
            if(Input.GetMouseButtonDown(0) && tile.CanBuild())
            {
                BuildTower(tile);
            }
        }

        void OnBuildingSelected_Delegate(TowerBundle towerBundle)
        {
            _selectedTower = towerBundle.towerPrefab;
            _ghostFilter.mesh = towerBundle.combinedMesh;
        }

        void BuildTower(HexTile tile)
        {
            if(!tile.CanBuild())
            {
                Debug.LogError("Trying to build on invalid tile!");
                return;
            }
            
            var tower = Instantiate(_selectedTower, tile.transform.position, Quaternion.identity
                , towersParent);
            tile.SetBuilding(tower);
        }
    }
}