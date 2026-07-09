using UnityEngine;

public class LowObstacle : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (sprite == null)
        {
            sprite = gameObject.AddComponent<SpriteRenderer>();
            sprite.sprite = CreateBarrierSprite();
            sprite.sortingLayerName = "Default";
        }

        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.size = new Vector2(1.2f, 0.8f);
        }
        boxCollider.isTrigger = true;
    }

    void Start()
    {
        DamageDealer dd = gameObject.AddComponent<DamageDealer>();
        dd.damage = 1;
        dd.bypassWhenSliding = true;
    }

    Sprite CreateBarrierSprite()
    {
        Texture2D tex = new Texture2D(16, 16);
        Color[] pixels = new Color[256];
        Color orange = new Color(1f, 0.5f, 0f);
        Color stripe = new Color(1f, 0.7f, 0f);
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % 16;
            int y = i / 16;
            bool isStripe = (x / 2 + y) % 2 == 0;
            pixels[i] = isStripe ? stripe : orange;
        }
        tex.SetPixels(pixels);
        tex.wrapMode = TextureWrapMode.Repeat;
        tex.filterMode = FilterMode.Point;
        tex.Apply();
        return Sprite.Create(tex, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 32f);
    }
}
