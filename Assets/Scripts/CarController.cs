using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 4f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (sprite == null)
        {
            sprite = gameObject.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateCarSprite();
            sprite.sortingLayerName = "Default";
        }

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.freezeRotation = true;
        }

        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D col = gameObject.AddComponent<BoxCollider2D>();
            col.size = new Vector2(2f, 0.8f);
        }
    }

    void Start()
    {
        DamageDealer dd = gameObject.AddComponent<DamageDealer>();
        dd.damage = 1;
        dd.bypassWhenSliding = false;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + Vector2.left * speed * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (transform.position.x < -15f)
            Destroy(gameObject);
    }

    Sprite CreateCarSprite()
    {
        Texture2D tex = new Texture2D(32, 16);
        Color[] pixels = new Color[512];
        Color red = new Color(0.8f, 0.1f, 0.1f);
        Color darkRed = new Color(0.5f, 0.05f, 0.05f);
        Color glass = new Color(0.6f, 0.8f, 1f, 0.7f);
        Color black = Color.black;
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % 32;
            int y = i / 32;
            if ((x >= 2 && x <= 5 && y >= 1 && y <= 5) || (x >= 26 && x <= 29 && y >= 1 && y <= 5))
                pixels[i] = black;
            else if (x >= 8 && x <= 16 && y >= 2 && y <= 7)
                pixels[i] = glass;
            else if (y >= 1 && y <= 8 && x >= 4 && x <= 27)
                pixels[i] = (y >= 4 && y <= 6 && x >= 8 && x <= 16) ? glass : red;
            else
                pixels[i] = darkRed;
        }
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 32, 16), new Vector2(0.5f, 0.5f), 32f);
    }
}
