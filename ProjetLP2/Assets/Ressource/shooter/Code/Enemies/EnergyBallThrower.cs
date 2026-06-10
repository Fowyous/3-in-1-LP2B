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
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            Debug.Log("M5: Fired Horizontal Laser!");
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

        // Phase intro : glisse vers la position de combat en X
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
            // Suit le joueur verticalement
            float directionY = Mathf.Sign(diff);
            moveDirection = new Vector3(0, directionY, 0);
            rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
        }
        else
        {
            // Aligné : stoppe et lance le cycle de tir une seule fois
            rb.linearVelocity = Vector2.zero;
            if (!isLockedAndShooting)
                StartCoroutine(LockAndShootRoutine());
        }
    }

    ///<summary>
    ///Lance l'effet de charge, attend 1s, tire le laser, attend 3.5s puis reprend le tracking.
    ///</summary>
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
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null) baseScript.TakeDamage(Damage);
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}