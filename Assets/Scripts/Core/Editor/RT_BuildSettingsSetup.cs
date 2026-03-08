using UnityEditor;
using UnityEngine;

/// <summary>
/// Build Settings에 씬을 자동 등록하는 에디터 유틸리티
/// </summary>
public static class RT_BuildSettingsSetup
{
    [MenuItem("RiftTamers/Setup/씬 빌드 세팅 등록")]
    public static void RegisterScenes()
    {
        var scenes = new[]
        {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Dungeon.unity",
            "Assets/Scenes/MetaHub.unity",
        };

        var editorScenes = new EditorBuildSettingsScene[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            editorScenes[i] = new EditorBuildSettingsScene(scenes[i], true);
            Debug.Log($"[BuildSetup] 등록: [{i}] {scenes[i]}");
        }

        EditorBuildSettings.scenes = editorScenes;
        Debug.Log("[BuildSetup] Build Settings 씬 등록 완료!");
    }
}
