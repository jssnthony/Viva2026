using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerKick : MonoBehaviour
{
    [Header("Patada")]
    public float kickCooldown = 0.3f;
    public GameObject ballPrefab;

    private float cooldownTimer;
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.kKey.wasPressedThisFrame && cooldownTimer <= 0f)
            PerformKick();

        var mouse = Mouse.current;
        if (mouse != null && mouse.leftButton.wasPressedThisFrame && cooldownTimer <= 0f)
            PerformKick();
    }

    public void DoKick()
    {
        if (cooldownTimer <= 0f)
            PerformKick();
    }

    public void OnKick(InputValue value)
    {
        if (value.isPressed && cooldownTimer <= 0f)
            PerformKick();
    }

    void PerformKick()
    {
        cooldownTimer = kickCooldown;

        float dir = sprite != null && sprite.flipX ? -1f : 1f;
        Vector3 spawnPos = transform.position + new Vector3(dir * 0.8f, -0.3f, 0f);

        GameObject ball;
        if (ballPrefab != null)
        {
            ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            ball = new GameObject("Ball");
            ball.transform.position = spawnPos;
        }

        BallController bc = ball.GetComponent<BallController>();
        if (bc == null) bc = ball.AddComponent<BallController>();
        bc.Initialize(new Vector2(dir, 0f));
    }
}
