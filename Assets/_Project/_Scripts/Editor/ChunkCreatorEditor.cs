using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(ChunkCreator))]
    public class ChunkCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ChunkCreator myScript = (ChunkCreator)target;

            GUILayout.Label("There need to be 6 hex prefabs in the list to generate a chunk.", 
                EditorStyles.wordWrappedLabel);
            
            if (GUILayout.Button("Generate Chunks"))
            {
                myScript.CreateChunk();
            }
        }
    }
}