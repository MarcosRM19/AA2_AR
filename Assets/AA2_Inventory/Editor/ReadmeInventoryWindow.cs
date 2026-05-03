using UnityEngine;
using UnityEditor;
using System.IO;

public class ReadmeInventoryWindow : EditorWindow
{
    private string content = "";
    private Vector2 scroll;

    public static void Open(string readmePath)
    {
        var window = GetWindow<ReadmeInventoryWindow>("AA2 Inventory - README");
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
