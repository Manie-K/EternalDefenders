using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(MapParent))]
    public class MapParentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MapParent myScript = (MapParent)target;

            if (GUILayout.Button("Delete map"))
            {
                myScript.DeleteMap();
            }
        }
    }
}