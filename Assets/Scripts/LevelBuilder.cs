using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init()
    {
        if (GameManager.Instance == null)
        {
            var gm = new GameObject("GameManager");
            gm.AddComponent<GameManager>();
        }
    }
}

public class LevelBuilder : MonoBehaviour
{
    private CameraController camCtrl;

    void Awake()
    {
        if (GameObject.Find("Player") != null) return;

        if (GameManager.Instance != null)
            GameManager.Instance.ForceLandscape();

        BuildLevel();
    }

    void BuildLevel()
    {
        GameManager gm = GameManager.Instance;
        if (gm != null) gm.StartGame();

        SetupCamera();
        Transform player = CreatePlayer();
        if (camCtrl != null) camCtrl.target = player;
        CreateGround();
        CreateLevelSections();
        CreateGoal();
        CreateUI();
    }

    void SetupCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camGO = new GameObject("MainCamera");
            cam = camGO.AddComponent<Camera>();
            camGO.tag = "MainCamera";
            camGO.AddComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        }
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.backgroundColor = new Color(0.3f, 0.5f, 0.7f);

        var urpData = cam.GetComponent<UnityEngine.Rendering.Universal.UniversalAdditionalCameraData>();
        if (urpData != null)
            urpData.renderPostProcessing = false;

        camCtrl = cam.gameObject.AddComponent<CameraController>();
        camCtrl.offset = new Vector3(3f, 1f, -10f);
        camCtrl.smoothSpeed = 4f;
    }

    Transform CreatePlayer()
    {
        Vector3 spawnPos = new Vector3(-7f, -2f, 0f);

        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.transform.position = spawnPos;

        SpriteRenderer sprite = player.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(16, 24, new Color(0f, 0.41f, 0.28f));
        sprite.sortingLayerName = "Default";

        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3.5f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.linearDamping = 0f;

        BoxCollider2D collider = player.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(0.7f, 1.4f);
        collider.sharedMaterial = CreateNoFrictionMaterial();

        GameObject check = new GameObject("GroundCheck");
        check.transform.parent = player.transform;
        check.transform.localPosition = new Vector3(0f, -0.8f, 0f);

        PlayerController controller = player.AddComponent<PlayerController>();
        controller.groundCheck = check.transform;
        controller.groundLayer = LayerMask.GetMask("Default");

        player.AddComponent<PlayerHealth>();

        player.AddComponent<PlayerKick>();

        return player.transform;
    }

    void CreateGround()
    {
        GameObject ground = new GameObject("Ground");
        ground.transform.position = new Vector3(40f, -5f, 0f);

        SpriteRenderer sprite = ground.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(32, 32, new Color(0.25f, 0.25f, 0.25f));
        sprite.drawMode = SpriteDrawMode.Tiled;
        sprite.size = new Vector2(160f, 2f);

        BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(160f, 2f);
        collider.offset = new Vector2(0f, 0f);
    }

    void CreateLevelSections()
    {
        CreateTutorialSection();
        CreateTrafficSection();
        CreatePoliceSection();
        CreateMixedSection();
        CreateFinalSection();
    }

    void CreateTutorialSection()
    {
        float startX = 0f;

        CreatePlatform(new Vector3(startX + 3f, -1f, 0f), new Vector2(2f, 0.4f));

        CreateTrafficObstacle(new Vector3(startX + 8f, -3.5f, 0f));

        CreateLowObstacle(new Vector3(startX + 14f, -2.8f, 0f));

        SpawnCar();
    }

    void CreateTrafficSection()
    {
        float startX = 18f;

        CreateTrafficObstacle(new Vector3(startX + 2f, -3.5f, 0f));
        CreateTrafficObstacle(new Vector3(startX + 5f, -3.5f, 0f));
        CreatePlatform(new Vector3(startX + 3f, 0f, 0f), new Vector2(2f, 0.4f));

        CreateLowObstacle(new Vector3(startX + 9f, -2.8f, 0f));

        CreateTrafficObstacle(new Vector3(startX + 12f, -3.5f, 0f));
        CreateTrafficObstacle(new Vector3(startX + 15f, -3.5f, 0f));
    }

    void CreatePoliceSection()
    {
        float startX = 34f;

        CreatePolice(new Vector3(startX + 3f, -4f, 0f), startX + 1f, startX + 6f);
        CreatePlatform(new Vector3(startX + 5f, -1f, 0f), new Vector2(3f, 0.4f));
        CreateLowObstacle(new Vector3(startX + 8f, -2.8f, 0f));

        CreatePolice(new Vector3(startX + 12f, -4f, 0f), startX + 10f, startX + 15f);
        CreateTrafficObstacle(new Vector3(startX + 15f, -3.5f, 0f));
    }

    void CreateMixedSection()
    {
        float startX = 50f;

        SpawnCar();
        CreateTrafficObstacle(new Vector3(startX + 3f, -3.5f, 0f));
        CreateLowObstacle(new Vector3(startX + 5f, -2.8f, 0f));
        CreatePolice(new Vector3(startX + 8f, -4f, 0f), startX + 6f, startX + 12f);

        SpawnCar();
        CreatePlatform(new Vector3(startX + 12f, 0f, 0f), new Vector2(2f, 0.4f));
        CreateTrafficObstacle(new Vector3(startX + 16f, -3.5f, 0f));
        CreateLowObstacle(new Vector3(startX + 18f, -2.8f, 0f));
    }

    void CreateFinalSection()
    {
        float startX = 68f;

        CreateTrafficObstacle(new Vector3(startX + 2f, -3.5f, 0f));
        CreateTrafficObstacle(new Vector3(startX + 4f, -3.5f, 0f));
        CreateLowObstacle(new Vector3(startX + 6f, -2.8f, 0f));
        CreatePolice(new Vector3(startX + 9f, -4f, 0f), startX + 7f, startX + 13f);
        SpawnCar();

        CreatePlatform(new Vector3(startX + 12f, -1f, 0f), new Vector2(3f, 0.4f));
    }

    void CreatePlatform(Vector3 position, Vector2 size)
    {
        GameObject platform = new GameObject("Platform");
        platform.transform.position = position;

        SpriteRenderer sprite = platform.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(32, 32, new Color(0.5f, 0.5f, 0.5f));
        sprite.drawMode = SpriteDrawMode.Tiled;
        sprite.size = size;

        BoxCollider2D collider = platform.AddComponent<BoxCollider2D>();
        collider.size = size;
    }

    void CreateTrafficObstacle(Vector3 position)
    {
        GameObject car = new GameObject("Traffic");
        car.transform.position = position;

        SpriteRenderer sprite = car.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(24, 16, new Color(0.8f, 0.1f, 0.1f));
        sprite.sortingLayerName = "Default";

        BoxCollider2D collider = car.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1.5f, 1f);
        collider.sharedMaterial = CreateNoFrictionMaterial();

        Rigidbody2D rb = car.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        DamageDealer dd = car.AddComponent<DamageDealer>();
        dd.damage = 1;
        dd.bypassWhenSliding = false;
    }

    void CreateLowObstacle(Vector3 position)
    {
        GameObject barrier = new GameObject("LowObstacle");
        barrier.transform.position = position;
        barrier.AddComponent<LowObstacle>();
    }

    void CreatePolice(Vector3 position, float leftBound, float rightBound)
    {
        GameObject police = new GameObject("Police");
        police.transform.position = position;

        PoliceController pc = police.AddComponent<PoliceController>();
        pc.leftBound = leftBound;
        pc.rightBound = rightBound;
    }

    void SpawnCar()
    {
        float spawnX = Camera.main != null ? Camera.main.transform.position.x + 12f : 15f;
        float yPos = -3.8f;

        GameObject car = new GameObject("MovingCar");
        car.transform.position = new Vector3(spawnX, yPos, 0f);

        CarController cc = car.AddComponent<CarController>();
    }

    void CreateGoal()
    {
        Vector3 pos = new Vector3(80f, -3f, 0f);

        GameObject platform = new GameObject("GoalPlatform");
        platform.transform.position = pos + new Vector3(0f, -0.5f, 0f);

        SpriteRenderer pfSprite = platform.AddComponent<SpriteRenderer>();
        pfSprite.sprite = CreateSolidSprite(32, 32, new Color(0.3f, 0.3f, 0.3f));
        pfSprite.drawMode = SpriteDrawMode.Tiled;
        pfSprite.size = new Vector2(4f, 0.6f);

        BoxCollider2D pfCollider = platform.AddComponent<BoxCollider2D>();
        pfCollider.size = new Vector2(4f, 0.6f);

        GameObject goal = new GameObject("Goal");
        goal.transform.position = pos;

        SpriteRenderer sprite = goal.AddComponent<SpriteRenderer>();
        sprite.sprite = CreateSolidSprite(24, 40, new Color(0f, 0.8f, 0.2f));
        sprite.sortingLayerName = "Default";

        BoxCollider2D trigger = goal.AddComponent<BoxCollider2D>();
        trigger.size = new Vector2(1.5f, 2f);
        trigger.isTrigger = true;

        goal.AddComponent<GoalController>();
    }

    void CreateUI()
    {
        GameObject ui = new GameObject("GameUI");
        ui.AddComponent<GameUI>();

        GameObject touchUI = new GameObject("TouchUI");
        touchUI.AddComponent<TouchUI>();

        GameObject restartHint = new GameObject("RestartHint");
        restartHint.transform.position = new Vector3(-1000f, -1000f, 0f);
    }

    PhysicsMaterial2D CreateNoFrictionMaterial()
    {
        var mat = new PhysicsMaterial2D("NoFriction");
        mat.friction = 0f;
        mat.bounciness = 0f;
        return mat;
    }

    Sprite CreateSolidSprite(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;
        texture.SetPixels(pixels);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), 32f, 0, SpriteMeshType.FullRect);
    }
}
