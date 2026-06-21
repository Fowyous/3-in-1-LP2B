using UnityEngine;
using static ShooterConstants;

public class Kamikaz : MonoBehaviour, IEnemy
{
    public float Health { get; set; } = 1;
    public float Damage { get; set; } = 5;
    public bool IsAlive { get; private set; } = true;
    private float SPEED { get; set; } = 7.0f;
    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;
    public GameObject Target { get; set; }
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        NextMove(Target);
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        Health -= damage;
        if (Health <= 0)
        {
            IsAlive = false;
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(1f); // M4 base health value
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        // Kamikaz acts as its own projectile, no ranged attack
    }

    public void NextMove(GameObject target)
    {
        if (rb == null || target == null) return;

        Vector3 toTarget = (target.transform.position - transform.position).normalized;
        float p1 = ShooterConstants.Phase1limit;
        float p2 = ShooterConstants.Phase2limit;

        if (transform.position.x >= p1)
        {
            moveDirection = Vector3.left;
            lastDirection = moveDirection;
        }
        else if (transform.position.x < p1 && transform.position.x >= p2)
        {
            moveDirection = toTarget;
            lastDirection = moveDirection;
        }
        else
        {
            moveDirection = lastDirection != Vector3.zero ? lastDirection : Vector3.left;
        }

        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
    }

    ///<summary>
    ///Detonates on contact with either the UFO or the base.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(Damage);
            Debug.Log("Kamikaz exploded on the player!");
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
                baseScript.TakeDamage(Damage);
            Debug.Log("Kamikaz exploded on the Base!");
            Destroy(gameObject);
        }
    }
}