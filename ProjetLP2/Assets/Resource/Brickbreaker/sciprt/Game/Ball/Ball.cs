using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controls a single ball in BrickBreaker: movement, bouncing off bricks/walls,
/// out-of-bounds destruction, and the temporary power-up states (one-shot,
/// healing, speed boost) that can be activated on it.
/// </summary>
public class Ball : MonoBehaviour
{
    private const float DeathThresholdY = -8f;
    private const float DeathThresholdX = 13f;

    public static Ball Instance { get; private set; }
    
    public static bool IsOneShot     { get; private set; } = false;
    public static bool IsHealingBall { get; private set; } = false;

    public Vector2 direction;
    public bool    countsAsLife = true;

    public AudioClip losePoint; 
    private AudioSource audioSource;

    private Rigidbody2D ballRigidbody;
    private float speed;
    private bool isDead = false;
    private int lastBounceFrame = -1;

    private void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        ballRigidbody.bodyType = RigidbodyType2D.Kinematic;
        speed = 40f;

        if (direction == Vector2.zero)
            direction = new Vector2(0f, -1f).normalized;

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool isOutOfBounds = transform.position.y < DeathThresholdY || transform.position.x > DeathThresholdX || transform.position.x < -DeathThresholdX;

        if (!isDead && isOutOfBounds)
        {
            isDead = true;
            Destroy(gameObject);
            return;
        }

        ballRigidbody.MovePosition(ballRigidbody.position + direction * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.frameCount == lastBounceFrame) return;

        lastBounceFrame = Time.frameCount;
        direction       = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;
    }

    /// <summary>Temporarily lets this ball destroy any brick in a single hit.</summary>
    public void ActivateOneShot(float duration)
    {
        StartCoroutine(TimedFlag(
            set: () => IsOneShot = true,
            reset: () => IsOneShot = false,
            duration));
    }

    /// <summary>Temporarily turns this ball into a healing ball (restores brick health instead of damaging it).</summary>
    public void ActivateHealingBall(float duration)
    {
        StartCoroutine(TimedFlag(
            set: () => IsHealingBall = true,
            reset: () => IsHealingBall = false,
            duration));
    }

    /// <summary>Temporarily increases this ball's speed by the given percentage.</summary>
    public void ActivateBallSpeedBoost(float percent, float duration)
    {
        StartCoroutine(TimedMultiplier(percent, duration));
    }

    /// <summary>Sets a flag on, waits, then sets it back off after the given duration.</summary>
    private IEnumerator TimedFlag(Action set, Action reset, float duration)
    {
        set();
        yield return new WaitForSeconds(duration);
        reset();
    }

    /// <summary>Temporarily boosts speed by a percentage, then removes exactly that bonus afterward.</summary>
    private IEnumerator TimedMultiplier(float percent, float duration)
    {
        float speedBonus = speed * (percent / 100f);
        speed += speedBonus;
        yield return new WaitForSeconds(duration);
        speed -= speedBonus;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnDestroy()
    {
        if (SpawnerBall.Instance != null)
            SpawnerBall.Instance.NotifyBallDestroyed(gameObject, countsAsLife);
    }
}