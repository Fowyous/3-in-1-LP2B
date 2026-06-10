using UnityEngine;
using System.Collections;

public class BossMonster : MonoBehaviour, IEnemy
{
    [Header("Boss Stats")]
    [SerializeField] private float health = 15f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float speed = 1.5f;

    [Header("Boss Arsenal")]
    [SerializeField] private GameObject electricPrefab;
    [SerializeField] private GameObject flamePrefab;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject sniperPrefab;

    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private int currentAttackPhase = 0;
    private LaserChargeEffect chargeEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        chargeEffect = GetComponent<LaserChargeEffect>();
        StartCoroutine(BossAttackCycle());
    }

    void Update()
    {
        NextMove(Target);
    }

    public void TakeDamage(float damageAmount)
    {
        if (!IsAlive) return;
        Health -= damageAmount;
        if (Health <= 0)
        {
            IsAlive = false;
            if (chargeEffect != null) chargeEffect.StopChargeEffect();
            Destroy(gameObject);
        }
    }

    public void Shoot(GameObject bullet)
    {
        if (bullet == null || firePoint == null) return;
        GameObject projectile = Instantiate(bullet, firePoint.position, firePoint.rotation);
        FlameCone flameCone = projectile.GetComponent<FlameCone>();
        if (flameCone != null && Target != null)
            flameCone.SetTarget(Target);
    }

    public void NextMove(GameObject target)
    {
        if (rb == null || !IsAlive) return;
        if (transform.position.x > 4.0f)
            rb.linearVelocity = Vector3.left * speed;
        else
        {
            float hoverY = Mathf.Sin(Time.time * 2f) * speed;
            rb.linearVelocity = new Vector2(0, hoverY);
        }
    }

    private IEnumerator BossAttackCycle()
    {
        yield return new WaitForSeconds(2f);

        while (IsAlive)
        {
            currentAttackPhase = Random.Range(0, 4);

            switch (currentAttackPhase)
            {
                case 0:
                    Debug.Log("Boss: M1 Electric Ball!");
                    Shoot(electricPrefab);
                    break;

                case 1:
                    Debug.Log("Boss: M2 Flame Burst!");
                    for (int i = 0; i < 2; i++)
                    {
                        Shoot(flamePrefab);
                        yield return new WaitForSeconds(0.3f);
                    }
                    break;

                case 2:
                    Debug.Log("Boss: M5 Laser — charging...");
                    // Déclenche l'animation de chargement avant le tir
                    if (chargeEffect != null) chargeEffect.PlayChargeEffect();
                    yield return new WaitForSeconds(1.0f); // Doit correspondre à chargeDuration
                    Shoot(laserPrefab);
                    break;

                case 3:
                    Debug.Log("Boss: M6 Sniper Burst!");
                    for (int i = 0; i < 4; i++)
                    {
                        Shoot(sniperPrefab);
                        yield return new WaitForSeconds(0.12f);
                    }
                    break;
            }

            yield return new WaitForSeconds(4f);
        }
    }
}