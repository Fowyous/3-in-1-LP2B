using UnityEngine;
using System.Collections;
using static ShooterConstants;

public class SniperMonster : MonoBehaviour, IEnemy
{
    [Header("Enemy Stats")]
    [SerializeField] private float health = 2f;
    [SerializeField] private float damage = 0.25f;
    [SerializeField] private float speed = 5f;

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
    private int currentAmmo = 30;
    private float internalFireTimer = 0f;

    [Header("Base Collision Damage")]
    [Tooltip("Contact damage dealt to the base on crash, since the Sniper has no real melee damage stat.")]
    [SerializeField] private float baseContactDamage = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        NextMove(Target);

        if (transform.position.x < ShooterConstants.Phase1limit &&
            transform.position.x >= ShooterConstants.Phase2limit)
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

    private IEnumerator RechargeWeapon()
    {
        isRecharging = true;
        yield return new WaitForSeconds(3f);
        currentAmmo = 30;
        isRecharging = false;
    }

    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0 && IsAlive)
        {
            IsAlive = false;
            if (ScoreManager.Instance != null) ScoreManager.Instance.AddScore(2f); // M6 base health value
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        if (Time.time >= internalFireTimer && bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            currentAmmo--;
            internalFireTimer = Time.time + 0.1f;
        }
    }

    public void NextMove(GameObject target)
    {
        if (rb == null) return;

        if (transform.position.x >= ShooterConstants.Phase2limit)
        {
            moveDirection = Vector3.left;
        }
        rb.linearVelocity = moveDirection * speed;
    }

    ///<summary>
    ///Handles collision with the UFO or the base.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(baseContactDamage);
            IsAlive = false;
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
                baseScript.TakeDamage(baseContactDamage);
            IsAlive = false;
            Destroy(gameObject);
        }
    }
}