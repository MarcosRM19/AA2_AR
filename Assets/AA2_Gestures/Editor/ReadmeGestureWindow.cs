using UnityEngine;
using UnityEditor;
using System.IO;

public class ReadmeGestureWindow : EditorWindow
{
    private string content = "";
    private Vector2 scroll;

    public static void Open(string readmePath)
    {
        var window = GetWindow<ReadmeGestureWindow>("AA2 Gesture - README");
        window.minSize = new Vector2(500, 400);
        window.content = File.ReadAllText(readmePath);
        window.Show();
    }

    void OnGUI()
    {
        scroll = EditorGUILayout.BeginScrollView(scroll);
        EditorGUILayout.TextArea(content, GUILayout.ExpandHeight(true));
        EditorGUILayout.EndScrollView();
    }
}
