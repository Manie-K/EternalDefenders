using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(PlayerResourceInventory))]
    public class PlayerResourceInventoryEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            PlayerResourceInventory inv = (PlayerResourceInventory)target;
            if(inv == null) return;
            
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            
            GUILayout.Space(6);
            GUILayout.Label("Player resources:".Bold(), richTextStyle);
            GUILayout.Space(2);
            
            var resources = inv.GetAllResources();
            if(resources == null) return;
            foreach((var resourceSo, int amount) in resources)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(resourceSo.Sprite.texture, GUILayout.Width(26), GUILayout.Height(26));
                GUILayout.Label($"{resourceSo.Name}: \t {amount}");
                GUILayout.EndHorizontal();
            }
        }
    }
}