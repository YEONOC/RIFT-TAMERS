using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// Milestone 1 — 씬 1번 작업: 3개 씬 기본 오브젝트를 자동 세팅하는 에디터 유틸리티
/// </summary>
public static class RT_SceneSetup
{
    [MenuItem("RiftTamers/Setup/전체 씬 기본 세팅")]
    public static void SetupAllScenes()
    {
        SetupMainMenu();
        SetupDungeon();
        SetupMetaHub();
        AssetDatabase.SaveAssets();
        Debug.Log("[SceneSetup] 3개 씬 세팅 완료!");
    }

    // ─────────────────────────────────────────
    // MainMenu 씬
    // ─────────────────────────────────────────
    static void SetupMainMenu()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity", OpenSceneMode.Single);

        // Camera
        var camGo = new GameObject("Main Camera");
        var cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.backgroundColor = new Color(0.07f, 0.07f, 0.12f);
        cam.tag = "MainCamera";
        camGo.AddComponent<AudioListener>();

        // Pixel Perfect Camera
        var ppc = camGo.AddComponent<PixelPerfectCamera>();
        ppc.assetsPPU = 16;
        ppc.refResolutionX = 1920;
        ppc.refResolutionY = 1080;

        // Directional Light
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // GameManager
        var gmGo = new GameObject("GameManager");
        gmGo.AddComponent<GameManager>();

        // Canvas
        var canvasGo = new GameObject("Canvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        // 타이틀 텍스트
        var titleGo = new GameObject("TitleText");
        titleGo.transform.SetParent(canvasGo.transform, false);
        var titleText = titleGo.AddComponent<Text>();
        titleText.text = "RIFT TAMERS";
        titleText.fontSize = 72;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = Color.white;
        var titleRect = titleGo.GetComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 0.7f);
        titleRect.anchorMax = new Vector2(0.5f, 0.7f);
        titleRect.sizeDelta = new Vector2(800f, 120f);
        titleRect.anchoredPosition = Vector2.zero;

        // 버튼 패널
        var panelGo = new GameObject("ButtonPanel");
        panelGo.transform.SetParent(canvasGo.transform, false);
        var panelRect = panelGo.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.4f);
        panelRect.anchorMax = new Vector2(0.5f, 0.4f);
        panelRect.sizeDelta = new Vector2(300f, 240f);
        panelRect.anchoredPosition = Vector2.zero;
        var layout = panelGo.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 16f;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleCenter;

        CreateButton(panelGo.transform, "Btn_NewGame",  "새 게임");
        CreateButton(panelGo.transform, "Btn_Continue", "계속하기");
        CreateButton(panelGo.transform, "Btn_Settings", "설정");
        CreateButton(panelGo.transform, "Btn_Quit",     "종료");

        EditorSceneManager.SaveScene(scene);
        Debug.Log("[SceneSetup] MainMenu 씬 세팅 완료");
    }

    // ─────────────────────────────────────────
    // Dungeon 씬
    // ─────────────────────────────────────────
    static void SetupDungeon()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/Dungeon.unity", OpenSceneMode.Single);

        // Camera
        var camGo = new GameObject("Main Camera");
        var cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.backgroundColor = new Color(0.05f, 0.05f, 0.08f);
        cam.tag = "MainCamera";
        camGo.AddComponent<AudioListener>();
        var ppc = camGo.AddComponent<PixelPerfectCamera>();
        ppc.assetsPPU = 16;
        ppc.refResolutionX = 1920;
        ppc.refResolutionY = 1080;

        // Directional Light
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Grid (타일맵 루트)
        var gridGo = new GameObject("Grid");
        gridGo.AddComponent<Grid>();

        // SpawnPoint
        var spawnGo = new GameObject("SpawnPoint");
        spawnGo.transform.position = Vector3.zero;

        // Canvas (HUD)
        var canvasGo = new GameObject("Canvas_HUD");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        // 상단 Floor 정보
        var floorInfoGo = new GameObject("Text_FloorInfo");
        floorInfoGo.transform.SetParent(canvasGo.transform, false);
        var floorText = floorInfoGo.AddComponent<Text>();
        floorText.text = "Zone 1 - Floor 1";
        floorText.fontSize = 28;
        floorText.alignment = TextAnchor.MiddleCenter;
        floorText.color = Color.white;
        var floorRect = floorInfoGo.GetComponent<RectTransform>();
        floorRect.anchorMin = new Vector2(0.5f, 1f);
        floorRect.anchorMax = new Vector2(0.5f, 1f);
        floorRect.pivot = new Vector2(0.5f, 1f);
        floorRect.sizeDelta = new Vector2(400f, 60f);
        floorRect.anchoredPosition = new Vector2(0f, -10f);

        // 좌하단 파티 HP 패널
        var partyPanelGo = new GameObject("Panel_PartyHP");
        partyPanelGo.transform.SetParent(canvasGo.transform, false);
        var partyRect = partyPanelGo.AddComponent<RectTransform>();
        partyRect.anchorMin = new Vector2(0f, 0f);
        partyRect.anchorMax = new Vector2(0f, 0f);
        partyRect.pivot = new Vector2(0f, 0f);
        partyRect.sizeDelta = new Vector2(320f, 180f);
        partyRect.anchoredPosition = new Vector2(16f, 16f);
        var img = partyPanelGo.AddComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.5f);

        // 우하단 스킬 아이콘 패널
        var skillPanelGo = new GameObject("Panel_SkillIcons");
        skillPanelGo.transform.SetParent(canvasGo.transform, false);
        var skillRect = skillPanelGo.AddComponent<RectTransform>();
        skillRect.anchorMin = new Vector2(1f, 0f);
        skillRect.anchorMax = new Vector2(1f, 0f);
        skillRect.pivot = new Vector2(1f, 0f);
        skillRect.sizeDelta = new Vector2(280f, 100f);
        skillRect.anchoredPosition = new Vector2(-16f, 16f);
        var skillImg = skillPanelGo.AddComponent<Image>();
        skillImg.color = new Color(0f, 0f, 0f, 0.5f);

        EditorSceneManager.SaveScene(scene);
        Debug.Log("[SceneSetup] Dungeon 씬 세팅 완료");
    }

    // ─────────────────────────────────────────
    // MetaHub 씬
    // ─────────────────────────────────────────
    static void SetupMetaHub()
    {
        var scene = EditorSceneManager.OpenScene("Assets/Scenes/MetaHub.unity", OpenSceneMode.Single);

        // Camera
        var camGo = new GameObject("Main Camera");
        var cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 5f;
        cam.backgroundColor = new Color(0.06f, 0.06f, 0.10f);
        cam.tag = "MainCamera";
        camGo.AddComponent<AudioListener>();
        var ppc = camGo.AddComponent<PixelPerfectCamera>();
        ppc.assetsPPU = 16;
        ppc.refResolutionX = 1920;
        ppc.refResolutionY = 1080;

        // Directional Light
        var lightGo = new GameObject("Directional Light");
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Canvas
        var canvasGo = new GameObject("Canvas");
        var canvas = canvasGo.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGo.AddComponent<CanvasScaler>();
        canvasGo.AddComponent<GraphicRaycaster>();

        // 상단 테이머 레벨/EXP 영역
        var tamerInfoGo = new GameObject("Panel_TamerInfo");
        tamerInfoGo.transform.SetParent(canvasGo.transform, false);
        var tamerRect = tamerInfoGo.AddComponent<RectTransform>();
        tamerRect.anchorMin = new Vector2(0.5f, 1f);
        tamerRect.anchorMax = new Vector2(0.5f, 1f);
        tamerRect.pivot = new Vector2(0.5f, 1f);
        tamerRect.sizeDelta = new Vector2(500f, 80f);
        tamerRect.anchoredPosition = new Vector2(0f, -16f);
        var tamerImg = tamerInfoGo.AddComponent<Image>();
        tamerImg.color = new Color(0f, 0f, 0f, 0.5f);
        var tamerText = new GameObject("Text_TamerLevel");
        tamerText.transform.SetParent(tamerInfoGo.transform, false);
        var tText = tamerText.AddComponent<Text>();
        tText.text = "Lv.1 테이머";
        tText.fontSize = 28;
        tText.alignment = TextAnchor.MiddleCenter;
        tText.color = Color.white;
        var tTextRect = tamerText.GetComponent<RectTransform>();
        tTextRect.anchorMin = Vector2.zero;
        tTextRect.anchorMax = Vector2.one;
        tTextRect.offsetMin = Vector2.zero;
        tTextRect.offsetMax = Vector2.zero;

        // 중앙 버튼 패널
        var centerPanelGo = new GameObject("Panel_Center");
        centerPanelGo.transform.SetParent(canvasGo.transform, false);
        var centerRect = centerPanelGo.AddComponent<RectTransform>();
        centerRect.anchorMin = new Vector2(0.5f, 0.5f);
        centerRect.anchorMax = new Vector2(0.5f, 0.5f);
        centerRect.sizeDelta = new Vector2(400f, 240f);
        centerRect.anchoredPosition = Vector2.zero;
        var centerLayout = centerPanelGo.AddComponent<VerticalLayoutGroup>();
        centerLayout.spacing = 16f;
        centerLayout.childForceExpandWidth = true;
        centerLayout.childForceExpandHeight = false;
        centerLayout.childAlignment = TextAnchor.MiddleCenter;

        CreateButton(centerPanelGo.transform, "Btn_Compendium", "크리처 도감");
        CreateButton(centerPanelGo.transform, "Btn_Bond",       "유대 시스템");
        CreateButton(centerPanelGo.transform, "Btn_SkillTree",  "스킬 트리");

        // 하단 런 시작 버튼
        var startBtnGo = CreateButton(canvasGo.transform, "Btn_StartRun", "런 시작");
        var startRect = startBtnGo.GetComponent<RectTransform>();
        startRect.anchorMin = new Vector2(0.5f, 0f);
        startRect.anchorMax = new Vector2(0.5f, 0f);
        startRect.pivot = new Vector2(0.5f, 0f);
        startRect.sizeDelta = new Vector2(300f, 70f);
        startRect.anchoredPosition = new Vector2(0f, 40f);

        EditorSceneManager.SaveScene(scene);
        Debug.Log("[SceneSetup] MetaHub 씬 세팅 완료");
    }

    // ─────────────────────────────────────────
    // 헬퍼
    // ─────────────────────────────────────────
    static GameObject CreateButton(Transform parent, string goName, string label)
    {
        var btnGo = new GameObject(goName);
        btnGo.transform.SetParent(parent, false);
        var img = btnGo.AddComponent<Image>();
        img.color = new Color(0.2f, 0.2f, 0.3f, 1f);
        btnGo.AddComponent<Button>();

        var rect = btnGo.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(280f, 60f);

        var textGo = new GameObject("Text");
        textGo.transform.SetParent(btnGo.transform, false);
        var text = textGo.AddComponent<Text>();
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        var textRect = textGo.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        return btnGo;
    }
}
