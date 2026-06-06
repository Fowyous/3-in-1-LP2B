using UnityEngine;
using System.Collections;
using static ShooterConstants;

///<summary>
///The EnergyBallThrower (M5) enemy. It tracks the player vertically, locks, charges, 
///and instantiates an independent horizontal laser prefab.
///</summary>
public class EnergyBallThrower : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    public float Health { get; set; } = 4;
    public float Damage { get; set; } = 2; // Pass-through requirement value
    public bool IsAlive { get; private set; } = true;
    private float SPEED { get; set; } = 3.0f;

    [Header("Shooting Setup")]
    [SerializeField] private GameObject laserPrefab; // Drag your HorizontalLaser prefab here
    [SerializeField] private Transform firePoint;

    private Vector3 moveDirection = Vector3.left;
    private bool intro = true;
    private static float introDuration = 2.0f;
    private float introProgress;
    private Vector2 startPos;

    private bool isLockedAndShooting = false;
    public GameObject Target { get; set; }
    private Rigidbody2D rb;

    void Start()
    {
        startPos = transform.position;
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
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Instantiates the independent laser prefab at the designated fire point.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            // Spawn the laser as a child or independent object in the scene
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            Debug.Log("M5: Fired a separate Horizontal Laser Prefab!");
        }
    }

    public void NextMove(GameObject target)
    {
        if (rb == null || target == null || !IsAlive) return;

        if (isLockedAndShooting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float positionY = ShooterConstants.Phase1limit + 1;

        if (intro)
        {
            introProgress += Time.deltaTime / introDuration;
            transform.position = Vector2.Lerp(startPos, new Vector2(positionY, transform.position.y), introProgress);
            if (introProgress >= 1f) intro = false;
        }
        else
        {
            float targetY = target.transform.position.y;

            if (Mathf.Abs(targetY - transform.position.y) > 0.15f)
            {
                float directionY = Mathf.Sign(targetY - transform.position.y);
                moveDirection = new Vector3(0, directionY, 0);
            }
            else
            {
                moveDirection = Vector3.zero;
                StartCoroutine(LockAndShootRoutine());
            }
        }

        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
    }

    private IEnumerator LockAndShootRoutine()
    {
        isLockedAndShooting = true;
        rb.linearVelocity = Vector2.zero;

        // 1 second charging lock delay
        yield return new WaitForSeconds(1.0f);

        // Fires the projectile laser prefab
        Shoot(laserPrefab);

        // Wait 3.5 seconds before being allowed to move/track again (time for laser to finish + cooldown)
        yield return new WaitForSeconds(3.5f);
        
        isLockedAndShooting = false;
    }
}