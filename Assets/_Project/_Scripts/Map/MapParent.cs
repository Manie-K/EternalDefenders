using MG_Utilities;
using UnityEngine;

namespace EternalDefenders
{
    public class MapParent : Singleton<MapParent>
    {
        public void DeleteMap()
        {
            if(Application.isPlaying)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                foreach (Transform child in transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}