using UnityEngine;
using System.Collections;
using static ShooterConstants;

///<summary>
///The EnergyBallThrower (M5) enemy. Tracks the player vertically, locks, charges,
///and instantiates an independent horizontal laser prefab.
///</summary>
public class EnergyBallThrower : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    public float Health { get; set; } = 4;
    public float Damage { get; set; } = 2;
    public bool IsAlive { get; private set; } = true;
    private float SPEED { get; set; } = 3.0f;

    [Header("Shooting Setup")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;

    private Vector3 moveDirection = Vector3.left;
    private bool intro = true;
    private static float introDuration = 2.0f;
    private float introProgress;
    private Vector2 startPos;

    private bool isLockedAndShooting = false;

    public GameObject Target { get; set; }
    private Rigidbody2D rb;
    private LaserChargeEffect chargeEffect;

    // Reference to the currently active laser, so it can be destroyed if this enemy dies mid-shot
    private GameObject activeLaser;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        chargeEffect = GetComponent<LaserChargeEffect>();
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
            if (chargeEffect != null) chargeEffect.StopChargeEffect();
            DestroyActiveLaser();
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            activeLaser = Instantiate(bullet, firePoint.position, firePoint.rotation);

            // Make the laser follow this enemy's firePoint every frame
            HorizontalLaser laserScript = activeLaser.GetComponent<HorizontalLaser>();
            if (laserScript != null)
            {
                laserScript.SetSource(firePoint);
            }

            Debug.Log("M5: Fired Horizontal Laser!");
        }
    }

    ///<summary>
    ///Destroys the currently active laser, if any. Called when this enemy dies.
    ///</summary>
    private void DestroyActiveLaser()
    {
        if (activeLaser != null)
        {
            Destroy(activeLaser);
            activeLaser = null;
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

        float combatPositionX = ShooterConstants.Phase1limit - 1f;

        if (intro)
        {
            introProgress += Time.deltaTime / introDuration;
            transform.position = Vector2.Lerp(
                startPos,
                new Vector2(combatPositionX, transform.position.y),
                introProgress
            );
            if (introProgress >= 1f) intro = false;
            return;
        }

        float targetY = target.transform.position.y;
        float diff = targetY - transform.position.y;

        if (Mathf.Abs(diff) > 0.15f)
        {
            float directionY = Mathf.Sign(diff);
            moveDirection = new Vector3(0, directionY, 0);
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            if (!isLockedAndShooting)
                StartCoroutine(LockAndShootRoutine());
        }
    }

    private IEnumerator LockAndShootRoutine()
    {
        isLockedAndShooting = true;
        rb.linearVelocity = Vector2.zero;

        if (chargeEffect != null) chargeEffect.PlayChargeEffect();
        yield return new WaitForSeconds(1.0f);

        Shoot(laserPrefab);
        yield return new WaitForSeconds(3.5f);

        isLockedAndShooting = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(Damage);
            IsAlive = false;
            DestroyActiveLaser();
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null) baseScript.TakeDamage(Damage);
            IsAlive = false;
            DestroyActiveLaser();
            Destroy(gameObject);
        }
    }
}