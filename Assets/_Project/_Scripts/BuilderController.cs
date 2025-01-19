using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class BuilderController : Singleton<BuilderController>
    {
        [SerializeField] TowerController towerPrefab;
        bool _isEnabled = false;
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.B))
                _isEnabled = !_isEnabled;
            
            if(!_isEnabled) return;

            if (Input.GetMouseButtonDown(0))
            {
                HexTile tile = HexMapController.Instance.GetHexTileFromWorldPosition(
                    CameraController.Instance.GetWorldMousePosition()
                    );

                if (tile.CanBuild())
                {
                    tile.BuildOnThisTile(towerPrefab);
                }
            }
        }
    }
}