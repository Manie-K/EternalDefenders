using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(TowerController), true)]
    public class TowerControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            TowerController myScript = (TowerController)target;

            int hp = myScript.Stats.GetStat(StatType.Health);
            
            GUILayout.Space(6);
            GUILayout.Label("Tower stats:".Bold(), richTextStyle);
            GUILayout.Space(2);
            GUILayout.Label("Health: \t".Bold() + hp.ToString().Italics(), richTextStyle);
        }
    }
}