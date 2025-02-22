using System;
using MG_Utilities;
using UnityEditor;
using UnityEngine;

namespace EternalDefenders.Editor
{
    [CustomEditor(typeof(StateMachineBrain), true)]
    public class StateMachineBrainEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(!Application.isPlaying) return;
            GUIStyle richTextStyle = new GUIStyle(EditorStyles.label) { richText = true };
            StateMachineBrain brain = (StateMachineBrain)target;

            string currentStateName = brain.CurrentState?.Name ?? "None";
            GUILayout.Label("Current State: \t".Bold() + currentStateName.Italics().Color("red"), richTextStyle);
        }
    }
}