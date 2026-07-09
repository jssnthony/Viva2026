using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 3;
    public float invulnerabilityTime = 1f;

    private int currentHealth;
    private SpriteRenderer sprite;
    private float invulnerabilityTimer;
    private bool isInvulnerable;
    private PlayerController playerController;

    void Awake()
    {
        currentHealth = maxHealth;
        sprite = GetComponent<SpriteRenderer>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;
            sprite.enabled = Mathf.FloorToInt(invulnerabilityTimer * 10) % 2 == 0;

            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                sprite.enabled = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (isInvulnerable) return;

        currentHealth -= amount;
        isInvulnerable = true;
        invulnerabilityTimer = invulnerabilityTime;

        if (GameManager.Instance != null)
            GameManager.Instance.LoseLife();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Lose();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        HandleDamage(other.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleDamage(other.gameObject);
    }

    void HandleDamage(GameObject source)
    {
        if (isInvulnerable) return;

        DamageDealer dd = source.GetComponent<DamageDealer>();
        if (dd == null) return;

        if (dd.bypassWhenSliding && playerController != null && playerController.IsSliding())
            return;

        TakeDamage(dd.damage);
    }
}
