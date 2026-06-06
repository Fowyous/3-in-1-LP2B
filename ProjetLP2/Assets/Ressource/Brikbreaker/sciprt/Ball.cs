using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float DeathThreshold = -8f;
    private bool isDead = false;
    private float speed;
    private Vector2 direction;
    private Rigidbody2D balle;
    private int lastBounceFrame = -1;
    public static Ball Instance { get; private set; }
    public bool countsAsLife = true;


    public static bool IsOneShot     { get; private set; } = false;
    public static bool IsHealingBall { get; private set; } = false;
    void Awake()
    {
        Instance = this;
    }
    protected virtual void Start()
    {
        balle = GetComponent<Rigidbody2D>();
        balle.bodyType = RigidbodyType2D.Kinematic;
        speed = 35f;
        direction = new Vector2(0f, -1f).normalized;
    }

    void Update()
    {
        if (!isDead && transform.position.y < DeathThreshold)
        {
            isDead = true;
            Destroy(gameObject);
            return;
        }

        balle.MovePosition(balle.position + direction * (speed * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.frameCount == lastBounceFrame) return;

        lastBounceFrame = Time.frameCount;
        direction = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;
    }
    
    public void ActivateOneShot(float duration)
    {
        StartCoroutine(TimedFlag(
            set: () => IsOneShot = true,
            reset: () => IsOneShot = false,
            duration));
    }

    public void ActivateHealingBall(float duration)
    {
        StartCoroutine(TimedFlag(
            set: () => IsHealingBall = true,
            reset: () => IsHealingBall = false,
            duration));
    }

    public void ActivateBallSpeedBoost(float percent, float duration)
    {
        StartCoroutine(TimedMultiplier(percent, duration));
    }

    private IEnumerator TimedFlag(System.Action set, System.Action reset, float duration)
    {
        set();
        yield return new WaitForSeconds(duration);
        reset();
    }

    private IEnumerator TimedMultiplier(float percent, float duration)
    {
        float speedBonus = speed *(percent / 100f);
        speed += speedBonus;
        yield return new WaitForSeconds(duration);
        speed -= speedBonus;
    }
}