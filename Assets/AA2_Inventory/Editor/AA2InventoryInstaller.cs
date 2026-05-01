using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[InitializeOnLoad]
public class AA2InventoryInstaller
{
    private const string INSTALLED_KEY = "AA2_Inventory_Installed";
    private const string SCENE_PATH = "Assets/AA2_Inventory/Scene/AA2_InventoryScene.unity";
    private const string README_PATH = "Assets/AA2_Inventory/README.md";

    static AA2InventoryInstaller()
    {
        EditorApplication.delayCall += OnPackageInstalled;
    }

    private static void OnPackageInstalled()
    {
        EditorPrefs.SetBool(INSTALLED_KEY, true);


        if (!File.Exists(SCENE_PATH))
        {
            return;
        }

        bool openScene = EditorUtility.DisplayDialog(
            "AA2 Inventory instalado ✓",
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

    // Menú para testear sin reinstalar
    [MenuItem("AA2 Inventory/Test Installer")]
    public static void TestInstaller()
    {
        EditorPrefs.DeleteKey(INSTALLED_KEY);
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

        ReadmeWindow.Open(README_PATH);
    }
}
