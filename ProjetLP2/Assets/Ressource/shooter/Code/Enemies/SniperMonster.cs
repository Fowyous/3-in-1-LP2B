using UnityEngine;
using System.Collections;
using static ShooterConstants;

public class SniperMonster : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 2f;      // M6 has 2 HP
    [SerializeField] private float damage = 0.25f;   // 0.25 damage per bullet
    [SerializeField] private float speed = 5f;       // High speed

    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    
    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private Vector3 moveDirection = Vector3.left;
    private bool isRecharging = false;
    private int currentAmmo = 30;                     // 30 round magazine
    
    // Moved outside the method to fix the 'static' modifier error
    private float internalFireTimer = 0f;

    ///<summary>
    ///Initializes Physics components.
    ///</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    ///<summary>
    ///Updates movement and handles rapid-fire logic inside Zone 2.
    ///</summary>
    void Update()
    {
        NextMove(Target);

        // Check weapon behavior in Zone 2
        if (transform.position.x < ShooterConstants.Phase1limit && transform.position.x >= ShooterConstants.Phase2limit)
        {
            if (currentAmmo > 0 && !isRecharging)
            {
                Shoot(bulletPrefab);
            }
            else if (currentAmmo <= 0 && !isRecharging)
            {
                StartCoroutine(RechargeWeapon());
            }
        }
    }

    ///<summary>
    ///Handles weapon reloads via a 3-second delay coroutine.
    ///</summary>
    private IEnumerator RechargeWeapon()
    {
        isRecharging = true;
        yield return new WaitForSeconds(3f); // 3 seconds reload time
        currentAmmo = 30;
        isRecharging = false;
    }

    ///<summary>
    ///Applies damage to the sniper.
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
    ///Fires a single rapid bullet and reduces ammo count based on frame rate timing.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        // Uses the class variable instead of an invalid local static variable
        if (Time.time >= internalFireTimer && bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            currentAmmo--;
            internalFireTimer = Time.time + 0.1f; // 10 bullets per second delay
        }
    }

    ///<summary>
    ///Movement patterns matching standard fast assault types.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null) return;
        
        // Stays mainly in Zone 2 or pushes forward
        if (transform.position.x >= ShooterConstants.Phase2limit)
        {
            moveDirection = Vector3.left;
        }
        rb.linearVelocity = moveDirection * speed;
    }
}