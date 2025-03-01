using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity;
using UnityEditor;

namespace EternalDefenders.Helpers
{
    public class MeshCombiner : MonoBehaviour
    {
        [SerializeField] GameObject target; 
        [SerializeField] string targetName; 
        public void CombineIntoMesh()
        {
            if(targetName == "")
                targetName = target.name + "_combinedMesh";
            
            List<MeshFilter> meshesToCombine = target.GetComponentsInChildren<MeshFilter>().ToList();
            var combine = new CombineInstance[meshesToCombine.Count];

            for(int i = 0; i < meshesToCombine.Count; i++)
            {
                combine[i].mesh = meshesToCombine[i].sharedMesh;
                combine[i].transform = Matrix4x4.TRS(meshesToCombine[i].transform.localPosition,
                    meshesToCombine[i].transform.rotation,
                    meshesToCombine[i].transform.localScale
                );
            }

            var mesh = new Mesh();
            mesh.CombineMeshes(combine);
            
            string path = $"Assets/_Project/Meshes/{targetName}.asset";
            
            AssetDatabase.CreateAsset(mesh, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}