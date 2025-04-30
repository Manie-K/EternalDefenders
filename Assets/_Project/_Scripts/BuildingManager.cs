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
        //public event Action OnBuildModeEnter;
        //public event Action OnBuildModeExit;
        public event Action OnBuildFinished;

        MeshFilter _ghostFilter;
        MeshRenderer _ghostRenderer;
        GameObject _ghost;
        TowerController _selectedTower;
        bool _isEnabled = false;
        bool _canPlaceBuilding = false;

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
            
            InputManager.Instance.OnBuildModeEnter += () =>
            {
                _isEnabled = true;
                _ghost.SetActive(true);
            };
            InputManager.Instance.OnBuildModeExit += () =>
            {
                _isEnabled = false;
                _selectedTower = null;
                _ghostFilter.mesh = null;
                _ghost.SetActive(false);
                _canPlaceBuilding = false;
            };

            Tower_Building_Panel_Controller.Instance.OnBuildingSelected += OnBuildingSelected_Delegate;
            InputManager.Instance.OnBuildingPlace += OnBuildingPlace_Delegate;
        }

        void OnDisable()
        {
            Tower_Building_Panel_Controller bcm = Tower_Building_Panel_Controller.Instance;
            if(bcm == null) return;
            bcm.OnBuildingSelected -= OnBuildingSelected_Delegate;
        }
        void Update()
        {
            if(!_isEnabled || _selectedTower == null) return;

            //Optimization: Might be a bit expensive
            HexTile tile = HexMapController.Instance.GetHexTileFromWorldPosition(
                   CameraController.Instance.GetWorldMousePosition()
               );
            
            _ghostRenderer.material.color = tile.CanBuild() ? ghostValidColor : ghostInvalidColor;
            _ghost.transform.position = tile.transform.position;

            if (_canPlaceBuilding && tile.CanBuild())
            {
                BuildTower(tile);
            }
        }

        void OnBuildingSelected_Delegate(TowerBundle towerBundle)
        {
            _selectedTower = towerBundle.towerPrefab;
            _ghostFilter.mesh = towerBundle.combinedMesh;
        }

        void OnBuildingPlace_Delegate()
        {
            _canPlaceBuilding = true;   
        }

        public void BuildTower(HexTile tile)
        {
            if(!tile.CanBuild())
            {
                Debug.LogError("Trying to build on invalid tile!");
                _canPlaceBuilding = false;
                OnBuildFinished?.Invoke();
                return;
            }
            
            var tower = Instantiate(_selectedTower, tile.transform.position.With(y: tile.BuildingHeight), Quaternion.identity
                , towersParent);
            tile.SetBuilding(tower);
            _canPlaceBuilding = false;
            OnBuildFinished?.Invoke();
        }
    }
}