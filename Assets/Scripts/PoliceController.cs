using UnityEngine;

public class PoliceController : MonoBehaviour
{
    [Header("Patrulla")]
    public float patrolSpeed = 2.5f;
    public float leftBound = -5f;
    public float rightBound = -2f;

    private SpriteRenderer sprite;
    private Rigidbody2D rb;
    private int direction = 1;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (sprite == null)
        {
            sprite = gameObject.AddComponent<SpriteRenderer>();
            sprite.sprite = CreatePoliceSprite();
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
            col.size = new Vector2(0.8f, 1.2f);
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
        Patrol();
    }

    void Patrol()
    {
        Vector2 targetPos = rb.position + Vector2.right * direction * patrolSpeed * Time.fixedDeltaTime;

        if (targetPos.x >= rightBound) direction = -1;
        else if (targetPos.x <= leftBound) direction = 1;

        targetPos = rb.position + Vector2.right * direction * patrolSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);

        if (sprite != null) sprite.flipX = direction < 0;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    Sprite CreatePoliceSprite()
    {
        Texture2D tex = new Texture2D(16, 24);
        Color[] pixels = new Color[384];
        Color blue = new Color(0.12f, 0.23f, 0.54f);
        Color skin = new Color(0.85f, 0.7f, 0.55f);
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % 16;
            int y = i / 16;
            if (y < 6) pixels[i] = blue;
            else if (y < 10) pixels[i] = skin;
            else if (y < 22) pixels[i] = blue;
            else pixels[i] = Color.black;
        }
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 16, 24), new Vector2(0.5f, 0.5f), 32f);
    }
}
