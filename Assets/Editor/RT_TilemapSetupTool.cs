using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

/// <summary>
/// 테스트용 타일맵 자동 생성 도구
/// 메뉴: RiftTamers > Tilemap > Setup Test Room
/// </summary>
public static class RT_TilemapSetupTool
{
    private const string TILESET_PATH = "Assets/Art/Tilesets";
    private const int TILE_SIZE = 16;

    // 타일 PPU: 16px 타일을 1 Unity unit으로 표시
    private const int TILE_PPU = TILE_SIZE;

    // 테스트 방 크기
    private const int ROOM_WIDTH  = 20;
    private const int ROOM_HEIGHT = 12;

    [MenuItem("RiftTamers/Tilemap/Apply Player Setup")]
    public static void ApplyPlayerSetup()
    {
        // 1) PixelPerfectCamera assetsPPU를 높여 줌인 효과 (16→48 : 약 3배 가까워짐)
        //    리플렉션 사용 — Editor 어셈블리에서 PixelPerfectCamera 직접 참조 불가
        var cam = Camera.main;
        if (cam != null)
        {
            var ppc = cam.GetComponent("PixelPerfectCamera") as Component;
            if (ppc != null)
            {
                var prop = ppc.GetType().GetProperty("assetsPPU");
                prop?.SetValue(ppc, 48);
                EditorUtility.SetDirty(ppc);
                Debug.Log("[RT_TilemapSetupTool] PixelPerfectCamera assetsPPU → 48 (3x 줌인)");
            }
            else
            {
                cam.orthographicSize = 3f;
                EditorUtility.SetDirty(cam);
                Debug.Log("[RT_TilemapSetupTool] 카메라 orthographicSize → 3");
            }
        }

        // 2) Player SpriteRenderer에 crystal_fox 첫 번째 프레임 적용
        var playerObj = GameObject.Find("Player");
        if (playerObj == null) { Debug.LogError("[RT_TilemapSetupTool] 'Player' 오브젝트를 찾을 수 없습니다."); return; }

        var sr = playerObj.GetComponent<SpriteRenderer>();
        if (sr == null) { Debug.LogError("[RT_TilemapSetupTool] SpriteRenderer가 없습니다."); return; }

        // 스프라이트 시트에서 첫 번째 스프라이트 로드
        var sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Art/Creatures/crystal_fox_1-Sheet.png");
        Sprite firstSprite = null;
        foreach (var asset in sprites)
        {
            if (asset is Sprite s) { firstSprite = s; break; }
        }

        if (firstSprite == null) { Debug.LogError("[RT_TilemapSetupTool] crystal_fox 스프라이트를 찾을 수 없습니다."); return; }

        sr.sprite = firstSprite;
        EditorUtility.SetDirty(playerObj);

        // 3) Animator 컨트롤러 적용
        var animator = playerObj.GetComponent<Animator>();
        if (animator != null)
        {
            var controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(
                "Assets/Art/Creatures/Player.controller");
            if (controller != null)
            {
                animator.runtimeAnimatorController = controller;
                EditorUtility.SetDirty(animator);
                Debug.Log("[RT_TilemapSetupTool] Animator 컨트롤러 적용 완료");
            }
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log($"[RT_TilemapSetupTool] Player 설정 완료 — 스프라이트: {firstSprite.name}");
    }

    [MenuItem("RiftTamers/Tilemap/Setup Test Room")]
    public static void SetupTestRoom()
    {
        // 1) 스프라이트 / 타일 에셋 생성
        Tile grassTile = GetOrCreateTile("Grass", CreateGrassTexture());
        Tile wallTile  = GetOrCreateTile("StoneWall", CreateStoneWallTexture());

        // 2) Grid 아래 Tilemap 레이어 확보
        Tilemap floorMap = GetOrCreateTilemap("TilemapFloor", 0);
        Tilemap wallMap  = GetOrCreateTilemap("TilemapWall",  1);

        // 3) 콜라이더 설정 (벽만)
        EnsureTilemapCollider(wallMap);

        // 4) 테스트 방 페인팅
        PaintTestRoom(floorMap, wallMap, grassTile, wallTile);

        // 5) 씬 더티 표시
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[RT_TilemapSetupTool] 테스트 방 생성 완료!");
    }

    // ──────────────────────────────────────────────────
    //  픽셀 아트 텍스처 생성
    // ──────────────────────────────────────────────────

    /// <summary>잔디 타일 텍스처 — 초록 계열 도트 패턴</summary>
    private static Texture2D CreateGrassTexture()
    {
        var tex = new Texture2D(TILE_SIZE, TILE_SIZE)
        {
            filterMode = FilterMode.Point,
            wrapMode   = TextureWrapMode.Clamp
        };

        Color baseGreen  = new Color(0.22f, 0.60f, 0.18f);
        Color darkGreen  = new Color(0.16f, 0.47f, 0.13f);
        Color lightGreen = new Color(0.35f, 0.72f, 0.28f);

        for (int y = 0; y < TILE_SIZE; y++)
        for (int x = 0; x < TILE_SIZE; x++)
        {
            bool dot = (x % 5 == 1 && y % 4 == 2) || (x % 7 == 3 && y % 6 == 0);
            Color c = dot
                ? (y % 2 == 0 ? lightGreen : darkGreen)
                : baseGreen;
            tex.SetPixel(x, y, c);
        }

        tex.Apply();
        return tex;
    }

    /// <summary>돌 벽 타일 텍스처 — 회색 벽돌 도트 패턴</summary>
    private static Texture2D CreateStoneWallTexture()
    {
        var tex = new Texture2D(TILE_SIZE, TILE_SIZE)
        {
            filterMode = FilterMode.Point,
            wrapMode   = TextureWrapMode.Clamp
        };

        Color baseGray  = new Color(0.50f, 0.50f, 0.52f);
        Color darkGray  = new Color(0.30f, 0.30f, 0.32f);
        Color lightGray = new Color(0.70f, 0.70f, 0.72f);

        for (int y = 0; y < TILE_SIZE; y++)
        for (int x = 0; x < TILE_SIZE; x++)
        {
            bool mortarH = (y == 0 || y == 8);
            bool mortarV = !mortarH && (y < 8 ? (x == 0 || x == 8) : (x == 4 || x == 12));

            Color c;
            if (mortarH || mortarV)
                c = darkGray;
            else if (x == 1 && y % 8 == 1)
                c = lightGray;
            else
                c = baseGray;

            tex.SetPixel(x, y, c);
        }

        tex.Apply();
        return tex;
    }

    // ──────────────────────────────────────────────────
    //  에셋 생성 헬퍼
    // ──────────────────────────────────────────────────

    private static Tile GetOrCreateTile(string tileName, Texture2D tex)
    {
        string spritePath = $"{TILESET_PATH}/{tileName}.png";
        string tilePath   = $"{TILESET_PATH}/{tileName}.asset";

        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(spritePath, bytes);
        AssetDatabase.ImportAsset(spritePath);

        var importer = (TextureImporter)AssetImporter.GetAtPath(spritePath);
        importer.textureType         = TextureImporterType.Sprite;
        importer.spritePixelsPerUnit = TILE_PPU;
        importer.filterMode          = FilterMode.Point;
        importer.textureCompression  = TextureImporterCompression.Uncompressed;
        importer.SaveAndReimport();

        Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
        if (tile == null)
        {
            tile = ScriptableObject.CreateInstance<Tile>();
            AssetDatabase.CreateAsset(tile, tilePath);
        }
        tile.sprite = sprite;
        EditorUtility.SetDirty(tile);
        AssetDatabase.SaveAssets();

        return tile;
    }

    // ──────────────────────────────────────────────────
    //  Tilemap GameObject 헬퍼
    // ──────────────────────────────────────────────────

    private static Tilemap GetOrCreateTilemap(string name, int sortingOrder)
    {
        var gridObj = GameObject.Find("Grid");
        if (gridObj == null)
        {
            gridObj = new GameObject("Grid");
            gridObj.AddComponent<Grid>();
        }

        Transform existing = gridObj.transform.Find(name);
        if (existing != null)
        {
            existing.GetComponent<TilemapRenderer>().sortingOrder = sortingOrder;
            return existing.GetComponent<Tilemap>();
        }

        var child = new GameObject(name);
        child.transform.SetParent(gridObj.transform, false);

        var tilemap  = child.AddComponent<Tilemap>();
        var renderer = child.AddComponent<TilemapRenderer>();
        renderer.sortingOrder = sortingOrder;

        return tilemap;
    }

    private static void EnsureTilemapCollider(Tilemap map)
    {
        if (map.GetComponent<TilemapCollider2D>() == null)
            map.gameObject.AddComponent<TilemapCollider2D>();

        if (map.GetComponent<CompositeCollider2D>() == null)
            map.gameObject.AddComponent<CompositeCollider2D>();

        var rb = map.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Static;

        var col = map.GetComponent<TilemapCollider2D>();
        col.compositeOperation = Collider2D.CompositeOperation.Merge;
    }

    // ──────────────────────────────────────────────────
    //  테스트 방 레이아웃 페인팅
    // ──────────────────────────────────────────────────

    /// <summary>
    /// ROOM_WIDTH x ROOM_HEIGHT 방:
    ///   테두리 = 돌 벽, 내부 = 잔디 바닥
    ///   상/하/좌/우 중앙 3칸 = 문 출구 (벽 없음)
    /// </summary>
    private static void PaintTestRoom(
        Tilemap floorMap, Tilemap wallMap,
        Tile grassTile,   Tile wallTile)
    {
        floorMap.ClearAllTiles();
        wallMap.ClearAllTiles();

        int halfW = ROOM_WIDTH  / 2;
        int halfH = ROOM_HEIGHT / 2;

        for (int x = -halfW; x < halfW; x++)
        for (int y = -halfH; y < halfH; y++)
        {
            var pos    = new Vector3Int(x, y, 0);
            bool edgeX = (x == -halfW || x == halfW - 1);
            bool edgeY = (y == -halfH || y == halfH - 1);

            if (edgeX || edgeY)
            {
                // 문 출구: 상하 중앙 3칸, 좌우 중앙 3칸
                bool isDoor = (edgeY && !edgeX && Mathf.Abs(x) <= 1)
                           || (edgeX && !edgeY && Mathf.Abs(y) <= 1);

                if (!isDoor)
                    wallMap.SetTile(pos, wallTile);
                else
                    floorMap.SetTile(pos, grassTile);
            }
            else
            {
                floorMap.SetTile(pos, grassTile);
            }
        }
    }
}
