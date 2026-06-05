using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float DeathThreshold = -8f;
    private bool isDead = false;
    private float speed;
    private Vector2 direction;
    private Rigidbody2D rb;
    private int lastBounceFrame = -1;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        speed = 32f;
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

        rb.MovePosition(rb.position + direction * (speed * Time.deltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.frameCount == lastBounceFrame) return;

        lastBounceFrame = Time.frameCount;
        direction = Vector2.Reflect(direction, collision.contacts[0].normal).normalized;
    }
}