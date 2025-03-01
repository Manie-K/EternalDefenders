using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(EnemyController), true)]
    public class EnemyControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(!Application.isPlaying) return;
            
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            EnemyController myScript = (EnemyController)target;

            int hp = myScript.Stats.GetStat(StatType.Health);
            int dmg = myScript.Stats.GetStat(StatType.Damage);
            
            GUILayout.Space(6);
            GUILayout.Label("Enemy stats:".Bold(), richTextStyle);
            GUILayout.Space(2);
            GUILayout.Label("Health: \t".Bold() + hp.ToString().Italics(), richTextStyle);
            GUILayout.Label("Damage: \t".Bold() + dmg.ToString().Italics(), richTextStyle);
        }
    }
}