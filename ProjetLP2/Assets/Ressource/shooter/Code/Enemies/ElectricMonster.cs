using UnityEngine;
using static ShooterConstants;

public class ElectricMonster : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 3f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float speed = 3f;

    [Header("Shooting Settings")]
    [SerializeField] private GameObject electricBallPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 2f;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;
    private float nextFireTime = 0f;

    // Smoothing: current vertical velocity used by MoveTowards to avoid snapping
    private float currentVelocityY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        NextMove(Target);

        // M1 only shoots in Zone 2
        if (transform.position.x < ShooterConstants.Phase1limit &&
            transform.position.x >= ShooterConstants.Phase2limit)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot(electricBallPrefab);
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0 && IsAlive)
        {
            IsAlive = false;
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
            Instantiate(bullet, firePoint.position, firePoint.rotation);
    }

    ///<summary>
    ///Zone 1 : avance tout droit vers la gauche.
    ///Zone 2 : suit le joueur verticalement avec MoveTowards pour éviter tout tremblement.
    ///Zone 3 : continue dans la dernière direction connue.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null) return;

        float p1 = ShooterConstants.Phase1limit;
        float p2 = ShooterConstants.Phase2limit;

        if (transform.position.x >= p1)
        {
            // Zone 1 : ligne droite vers la gauche
            moveDirection = Vector3.left;
            lastDirection = moveDirection;
        }
        else if (transform.position.x < p1 && transform.position.x >= p2)
        {
            // Zone 2 : tracking vertical lisse avec zone morte
            if (target != null)
            {
                float targetY = target.transform.position.y;
                float currentY = transform.position.y;
                float diff = targetY - currentY;

                if (Mathf.Abs(diff) > 0.3f)
                {
                    // MoveTowards : avance vers la cible sans jamais la dépasser ni osciller
                    currentVelocityY = Mathf.MoveTowards(
                        currentVelocityY,
                        Mathf.Sign(diff) * speed,
                        speed * 4f * Time.deltaTime
                    );
                }
                else
                {
                    // Zone morte : on s'arrête proprement
                    currentVelocityY = Mathf.MoveTowards(currentVelocityY, 0f, speed * 4f * Time.deltaTime);
                }

                moveDirection = new Vector3(-0.5f, currentVelocityY / speed, 0f).normalized;
                lastDirection = moveDirection;
            }
        }
        else
        {
            // Zone 3 : continue dans la dernière direction connue vers la base
            moveDirection = lastDirection != Vector3.zero ? lastDirection : Vector3.left;
        }

        rb.linearVelocity = moveDirection * speed;
    }
}