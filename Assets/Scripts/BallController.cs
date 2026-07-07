using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Balón")]
    public float speed = 18f;
    public float lifetime = 3f;

    private Vector2 direction;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();

        if (sprite == null)
        {
            sprite = gameObject.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateBallSprite();
            sprite.sortingLayerName = "Default";
        }

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }

        if (GetComponent<Collider2D>() == null)
        {
            CircleCollider2D col = gameObject.AddComponent<CircleCollider2D>();
            col.radius = 0.25f;
            col.isTrigger = true;
        }
    }

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        if (rb != null)
            rb.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PoliceController police = other.GetComponent<PoliceController>();
        if (police != null)
        {
            police.Die();
            Destroy(gameObject);
            return;
        }

        DamageDealer dd = other.GetComponent<DamageDealer>();
        if (dd != null)
        {
            Destroy(gameObject);
        }
    }

    Sprite CreateBallSprite()
    {
        Texture2D tex = new Texture2D(8, 8);
        Color[] pixels = new Color[64];
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % 8;
            int y = i / 8;
            float dx = x - 3.5f;
            float dy = y - 3.5f;
            float dist = Mathf.Sqrt(dx * dx + dy * dy);
            if (dist <= 3.5f)
                pixels[i] = (dist > 2.5f) ? Color.black : Color.white;
            else
                pixels[i] = Color.clear;
        }
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 16f);
    }
}
