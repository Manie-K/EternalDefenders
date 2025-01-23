using System.Collections.Generic;
using MG_Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace EternalDefenders
{
    public class BuilderManager : Singleton<BuilderManager>
    {
        [SerializeField] List<TowerController> towers;
        
        bool _isEnabled = false;
        int _currentIndex = 0;
        
        //TODO connect to input manager
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
                _isEnabled = !_isEnabled;
            
            if(!_isEnabled) return;
            
            if(Input.GetMouseButtonDown(1))
                _currentIndex = (_currentIndex + 1) % towers.Count;

            if(Input.GetMouseButtonDown(0))
            {
                HexTile tile = HexMapController.Instance.GetHexTileFromWorldPosition(
                    CameraController.Instance.GetWorldMousePosition()
                    );

                if (tile.CanBuild())
                {
                    tile.BuildOnThisTile(towers[_currentIndex]);
                }
            }
        }
    }
}