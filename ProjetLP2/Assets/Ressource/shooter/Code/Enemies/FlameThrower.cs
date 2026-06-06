using UnityEngine;
using static ShooterConstants;

///<summary>
///The flamethrower enemy. Follows the player from their front and starts shooting when in range.
///</summary>
public class FlameThrower : MonoBehaviour, IEnemy
{
    [Header("Enemy main variables")]
    [SerializeField] private float _health = 1f;
    [SerializeField] private float _damage = 5f;
    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private GameObject _target;
    
    [Header("Weapon Setup")]
    [SerializeField] private GameObject flameConePrefab;
    [SerializeField] private Transform firePoint;
    private float nextFireTime = 0f;
    private float fireRate = 1.5f;

    [Space(10)]
    [Header("FlameThrower specific variables")]
    [Tooltip("The distance to maintain to the right of the player")]
    [SerializeField] private float distancetomaintain = 2.0f;

    private bool intro = true;

    // Variables used to stabilize vertical variations and prevent jittering
    private float randomYOffset = 0f;
    private float offsetTimer = 0f;

    public float Health
    {
        get => _health;
        set => _health = value;
    }

    public float Damage
    {
        get => _damage;
        set => _damage = value;
    }

    public float SPEED
    {
        get => _speed;
        set => _speed = value;
    }

    public GameObject Target
    {
        get => _target;
        set => _target = value;
    }
    
    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;

    public bool IsAlive { get; private set; } = true;
    private Rigidbody2D rb;

    ///<summary>
    ///Initializes physics and references.
    ///</summary>
    void Start()
    {
        Debug.Log("FlameThrower spawned");
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("RIGIDBODY NOT FOUND! Make sure this GameObject has a Rigidbody component.");
        }
    }

    ///<summary>
    ///Updates AI combat behaviors and shooting checks.
    ///</summary>
    void Update()
    {
        // Safety check: wait until the spawner injects the target reference
        if (Target == null) return;

        NextMove(Target);

        if (!intro && Vector3.Distance(transform.position, Target.transform.position) <= 4.0f)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(flameConePrefab);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    ///<summary>
    ///Applies health reduction and handles monster death.
    ///</summary>
    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        Health -= damage;
        Debug.Log($"FlameThrower took {damage} damage. Health: {Health}");

        if (Health <= 0)
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Instantiates the flame cone projectile at the designated fire point.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    ///<summary>
    ///Determines tracking movement vectors toward the player's front position with a stabilized variation interval.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null || target == null) return;

        // Update the vertical offset every 1 second instead of every single frame to prevent shaking
        offsetTimer += Time.deltaTime;
        if (offsetTimer >= 1.0f)
        {
            randomYOffset = Random.Range(-1.5f, 1.5f);
            offsetTimer = 0f;
        }

        Vector3 targetPosition = target.transform.position;
        
        // Compute the final target coordinates with a fixed horizontal margin and stabilized vertical shifting
        Vector3 desiredPosition = new Vector3(
            targetPosition.x + distancetomaintain, 
            targetPosition.y + randomYOffset, 
            0f
        );

        Vector3 toTarget = desiredPosition - transform.position;

        float p1 = ShooterConstants.Phase1limit;
        
        if (transform.position.x >= p1 && intro)
        {
            moveDirection = Vector3.left;
            lastDirection = moveDirection;
        }
        else if (transform.position.x < p1 && intro)
        {
            intro = false;
            Debug.Log("FlameThrower: Intro completed, entering combat state.");
        }
        else
        {
            // Deadzone security: if the enemy is precisely close enough to its desired anchor point, stop pushing forces
            if (toTarget.magnitude < 0.15f)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            moveDirection = toTarget.normalized;
            lastDirection = moveDirection;
        }
        
        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
    }

    ///<summary>
    ///Detects collision contact with either the player UFO or the defensive Base structure.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Case 1: Crashing into the Player UFO
        if (collision.CompareTag("Joueur"))
        {
            UFO player = collision.GetComponent<UFO>();
            if (player != null)
            {
                player.TakeDamage(Damage);
                Debug.Log("FlameThrower crashed into the Player UFO!");
            }
            IsAlive = false;
            Destroy(gameObject);
        }
        // Case 2: Crashing into the allied defensive Base
        else if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
            {
                // Optional: Apply base damage here if BaseManager supports it
                // baseScript.TakeDamage(Damage);
                Debug.Log("FlameThrower crashed into the defensive Base!");
            }
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}