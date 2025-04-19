using MG_Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace EternalDefenders
{
    public class MapParent : Singleton<MapParent>
    {
        public void DeleteMap()
        {
            List<Transform> children = new List<Transform>();
            foreach (Transform child in transform)
            {
                children.Add(child);
            }

            if (Application.isPlaying)
            {
                foreach (Transform child in children)
                {
                    Destroy(child.gameObject);
                }
            }
            else
            {
                foreach (Transform child in children)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}