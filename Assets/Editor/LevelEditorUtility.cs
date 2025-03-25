#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class SetLevelWindow : EditorWindow {
    private int levelToSet = 1; // reset value

    [MenuItem("Tools/Set Level")]
    public static void ShowWindow() {
        GetWindow<SetLevelWindow>("Set Level");
    }

    private void OnGUI() {
        GUILayout.Label("Set Last Played Level", EditorStyles.boldLabel);

        levelToSet = EditorGUILayout.IntSlider("Level", levelToSet, 1, 10);

        if (GUILayout.Button("Apply")) {
            PlayerPrefs.SetInt("Level", levelToSet);
            Debug.Log("Last played level set to: " + levelToSet);
            Close();
        }
    }

}

#endif
