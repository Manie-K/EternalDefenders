using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(MapBuilder))]
    public class MapBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapBuilder myScript = (MapBuilder)target;

            if (GUILayout.Button("Generate Map"))
            {
                myScript.GenerateChunks();
            }
        }
    }
}