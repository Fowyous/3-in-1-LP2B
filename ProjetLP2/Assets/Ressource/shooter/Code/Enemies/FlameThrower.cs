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

    private float randomYOffset = 0f;
    private float offsetTimer = 0f;

    public float Health { get => _health; set => _health = value; }
    public float Damage { get => _damage; set => _damage = value; }
    public float SPEED { get => _speed; set => _speed = value; }
    public GameObject Target { get => _target; set => _target = value; }

    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;

    public bool IsAlive { get; private set; } = true;
    private Rigidbody2D rb;

    void Start()
    {
        Debug.Log("FlameThrower spawned");
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("RIGIDBODY NOT FOUND!");
    }

    void Update()
    {
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

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        Health -= damage;
        if (Health <= 0)
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Instantiates the flame cone projectile and injects the target reference so it can travel far enough.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            GameObject cone = Instantiate(bullet, firePoint.position, firePoint.rotation);

            // Inject target so the FlameCone knows how far it needs to travel
            FlameCone flameCone = cone.GetComponent<FlameCone>();
            if (flameCone != null && Target != null)
            {
                flameCone.SetTarget(Target);
            }
        }
    }

    public void NextMove(GameObject target)
    {
        if (rb == null || target == null) return;

        offsetTimer += Time.deltaTime;
        if (offsetTimer >= 1.0f)
        {
            randomYOffset = Random.Range(-1.5f, 1.5f);
            offsetTimer = 0f;
        }

        Vector3 targetPosition = target.transform.position;
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
        }
        else
        {
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
    ///Handles collision with the UFO player: applies contact damage + burn effect.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Case 1: Collision with the player UFO
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(Damage);
            player.ApplyBurn(0.5f, 4f); // Apply burn on contact crash
            Debug.Log("FlameThrower crashed into the Player UFO and applied burn!");
            IsAlive = false;
            Destroy(gameObject);
            return;
        }

        // Case 2: Collision with the base
        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
                baseScript.TakeDamage(Damage);
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}