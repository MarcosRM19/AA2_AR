using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public class AA2GesturesInstaller
{
    private const string SCENE_PATH = "Assets/AA2_Gestures/Scene/AA2_GestureScene.unity";
    private const string README_PATH = "Assets/AA2_Gestures/README.md";

    static AA2GesturesInstaller()
    {
        EditorApplication.delayCall += OnPackageInstalled;
    }

    private static void OnPackageInstalled()
    {
        if (!File.Exists(SCENE_PATH))
        {
            return;
        }

        bool openScene = EditorUtility.DisplayDialog(
            "AA2 Gestures instalado ✓",
            "¿Quieres abrir la escena de demostración?\n\nSe añadirá como primera escena en Build Settings.",
            "Sí, abrir escena",
            "No, gracias"
        );

        if (openScene)
        {
            AddSceneToBuildSettings(SCENE_PATH);
            EditorSceneManager.OpenScene(SCENE_PATH);
        }

        OpenReadme();
    }

    [MenuItem("AA2 Gesture/Test Installer")]
    public static void TestInstaller()
    {
        OnPackageInstalled();
    }

    private static void AddSceneToBuildSettings(string scenePath)
    {
        var scenes = EditorBuildSettings.scenes;
        foreach (var s in scenes)
            if (s.path == scenePath) return;

        var newScenes = new EditorBuildSettingsScene[scenes.Length + 1];
        newScenes[0] = new EditorBuildSettingsScene(scenePath, true);
        scenes.CopyTo(newScenes, 1);
        EditorBuildSettings.scenes = newScenes;
    }

    private static void OpenReadme()
    {
        if (!File.Exists(README_PATH))
        {
            return;
        }

        ReadmeGestureWindow.Open(README_PATH);
    }
}
