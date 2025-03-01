using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity;

namespace EternalDefenders.Helpers
{
    public static class MeshCombiner
    {
        public static Mesh CombineIntoMesh(GameObject target)
        {
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
            return mesh;
        }
    }
}