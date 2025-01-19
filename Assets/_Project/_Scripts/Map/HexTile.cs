using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class HexTile : MonoBehaviour
    {
        [SerializeField] bool canBuild = true;
        
        public TowerController Building { get; private set; }
        
        
        public bool CanBuild() => canBuild && Building is null;
        public void BuildOnThisTile(TowerController building)
        {
            if (!CanBuild())
            {
                Debug.Log("Cannot build on this tile");
                return;
            }
            Building = Instantiate(building, transform.position, Quaternion.identity);
        }
    }
}