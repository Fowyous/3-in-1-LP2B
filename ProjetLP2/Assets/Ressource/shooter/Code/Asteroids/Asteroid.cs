using UnityEngine;

///<summary>
///Decorative asteroid with parallax support.
///Speed and scale are set externally by AsteroidSpawner based on a depth layer,
///to create the illusion of foreground/background movement (parallax effect).
///</summary>
public class Asteroid : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed = 5f;     // Speed moving left (overridden by spawner)
    [SerializeField] private float rotationSpeed = 8f;  // Rotation speed

    [Header("Cleanup")]
    [Tooltip("X position past which the asteroid is destroyed to free memory.")]
    [SerializeField] private float destroyXThreshold = -12f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        rb.angularVelocity = rotationSpeed;
    }

    void Update()
    {
        // Cleanup: destroy once it has fully exited the screen on the left
        if (transform.position.x < destroyXThreshold)
        {
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Called by AsteroidSpawner right after instantiation to apply parallax depth values.
    ///</summary>
    public void SetParallaxValues(float speed, float scale)
    {
        moveSpeed = speed;
        transform.localScale = Vector3.one * scale;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
    }
}