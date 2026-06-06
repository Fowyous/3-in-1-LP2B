using UnityEngine;
using static ShooterConstants;

public class ElectricMonster : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 3f;      // M1 has 3 HP
    [SerializeField] private float damage = 2f;      // 2 damage per hit
    [SerializeField] private float speed = 3f;       // Average speed

    [Header("Shooting Settings")]
    [SerializeField] private GameObject electricBallPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;    // Shoots every 2 seconds

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;
    private float nextFireTime = 0f;

    ///<summary>
    ///Initializes components and checks references.
    ///</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    ///<summary>
    ///Updates movement and checks shooting capabilities if in Zone 2.
    ///</summary>
    void Update()
    {
        NextMove(Target);

        // M1 only shoots when in Zone 2 (between Phase1limit and Phase2limit)
        if (transform.position.x < ShooterConstants.Phase1limit && transform.position.x >= ShooterConstants.Phase2limit)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(electricBallPrefab);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    ///<summary>
    ///Applies damage to the monster and handles death.
    ///</summary>
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0 && IsAlive)
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Instantiates an electric projectile heading towards the player.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    ///<summary>
    ///Manages behavior across the three dynamic zones.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null || target == null) return;

        float p1 = ShooterConstants.Phase1limit;
        float p2 = ShooterConstants.Phase2limit;

        if (transform.position.x >= p1) // Zone 1: Move straight left
        {
            moveDirection = Vector3.left;
            lastDirection = moveDirection;
        }
        else if (transform.position.x < p1 && transform.position.x >= p2) // Zone 2: Follow player vertically
        {
            float directionY = Mathf.Sign(target.transform.position.y - transform.position.y);
            // Move left while tracking the player's Y position
            moveDirection = new Vector3(-0.5f, directionY, 0).normalized;
            lastDirection = moveDirection;
        }
        else // Zone 3: Rush straight into the base
        {
            moveDirection = lastDirection != Vector3.zero ? lastDirection : Vector3.left;
        }

        rb.linearVelocity = moveDirection * speed;
    }
}
