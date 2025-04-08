using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(MainBaseController))]
    public class MainBaseControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(!Application.isPlaying) return;
            
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            MainBaseController myScript = (MainBaseController)target;

            int hp = myScript.Stats.GetStat(StatType.Health);
            
            GUILayout.Space(6);
            GUILayout.Label("Main base stats:".Bold(), richTextStyle);
            GUILayout.Space(2);
            GUILayout.Label("Health: \t".Bold() + hp.ToString().Italics(), richTextStyle);
        }
    }
}