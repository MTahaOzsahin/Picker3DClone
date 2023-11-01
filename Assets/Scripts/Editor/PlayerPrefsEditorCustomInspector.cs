using Helpers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerPrefsEditor))]
    public class PlayerPrefsEditorCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var playerPrefsEditor = (PlayerPrefsEditor)target;

            if (GUILayout.Button("Load",GUILayout.Height(30)))
            {
                playerPrefsEditor.GetPlayerPrefs();
            }

            if (GUILayout.Button("Save",GUILayout.Height(30)))
            {
                playerPrefsEditor.SetPlayerPrefs();
            }
            
            EditorUtility.SetDirty(playerPrefsEditor);
        }
    }
}
