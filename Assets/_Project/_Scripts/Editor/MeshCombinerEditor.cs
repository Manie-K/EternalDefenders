using EternalDefenders.Helpers;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(MeshCombiner))]
    public class MeshCombinerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(!Application.isEditor)
                return;
            
            MeshCombiner myScript = (MeshCombiner)target;
            
            if (GUILayout.Button("Combine Meshes"))
            {
                myScript.CombineIntoMesh();
            }
        }
    }
}