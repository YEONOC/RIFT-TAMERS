using UnityEditor;
using UnityEditor.SceneManagement;

/// <summary>
/// 씬을 에디터에서 빠르게 여는 유틸리티
/// </summary>
public static class RT_SceneOpener
{
    [MenuItem("RiftTamers/Scenes/MainMenu 열기")]
    public static void OpenMainMenu()
        => EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

    [MenuItem("RiftTamers/Scenes/Dungeon 열기")]
    public static void OpenDungeon()
        => EditorSceneManager.OpenScene("Assets/Scenes/Dungeon.unity");

    [MenuItem("RiftTamers/Scenes/MetaHub 열기")]
    public static void OpenMetaHub()
        => EditorSceneManager.OpenScene("Assets/Scenes/MetaHub.unity");
}
