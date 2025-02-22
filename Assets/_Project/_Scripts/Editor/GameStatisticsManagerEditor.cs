using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(GameStatisticsManager))]
    public class GameStatisticsManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            GameStatisticsManager myScript = (GameStatisticsManager)target;

            
            GUILayout.Space(10);
            GUILayout.Label("Current game statistics:");
            GUILayout.Space(6);
            GUILayout.Label("Towers destroyed: \t".Bold() + myScript.TowersDestroyed.ToString().Italics(), richTextStyle);
            GUILayout.Label("Player deaths: \t".Bold() + myScript.PlayerDeaths.ToString().Italics(), richTextStyle);
            GUILayout.Label("Enemies killed: \t".Bold() + myScript.EnemiesKilled.ToString().Italics(), richTextStyle);
        }
    }
}