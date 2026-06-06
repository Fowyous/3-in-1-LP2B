using UnityEngine;
using System.Collections;

///<summary>
///The M3 Boss Monster. Has 15 HP, moves slowly into position, 
///and cycles randomly through M1, M2, M5, and M6 attack patterns every 4 seconds.
///</summary>
public class BossMonster : MonoBehaviour, IEnemy
{
    [Header("Boss Stats")]
    [SerializeField] private float health = 15f;    // 15 HP per specifications
    [SerializeField] private float damage = 5f;     // Contact/Collision damage
    [SerializeField] private float speed = 1.5f;    // Slow movement

    [Header("Boss Arsenal (Projectiles)")]
    [SerializeField] private GameObject electricPrefab; // M1 Projectile
    [SerializeField] private GameObject flamePrefab;    // M2 Projectile
    [SerializeField] private GameObject laserPrefab;    // M5 Projectile (Horizontal Laser)
    [SerializeField] private GameObject sniperPrefab;   // M6 Projectile

    [Header("Weapon Setup")]
    [SerializeField] private Transform firePoint;

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private int currentAttackPhase = 0;

    ///<summary>
    ///Initializes physics components and starts the periodic attack routine.
    ///</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BossAttackCycle());
    }

    ///<summary>
    ///Updates boss movement behaviors every frame.
    ///</summary>
    void Update()
    {
        NextMove(Target);
    }

    ///<summary>
    ///Applies damage to the Boss and handles its destruction.
    ///</summary>
    public void TakeDamage(float damageAmount)
    {
        if (!IsAlive) return;

        Health -= damageAmount;
        Debug.Log($"Boss took {damageAmount} damage. Health remaining: {Health}/15");

        if (Health <= 0)
        {
            IsAlive = false;
            Debug.Log("BOSS DESTROYED! Victory!");
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Required by IEnemy contract. Fires the projectile selected by the attack routine.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        if (bullet != null && firePoint != null)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }

    ///<summary>
    ///The boss moves slowly forward into Zone 2, then hovers smoothly up and down in place.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null || !IsAlive) return;

        // Move left until it reaches its combat position (X = 4f for example)
        if (transform.position.x > 4.0f)
        {
            rb.linearVelocity = Vector3.left * speed;
        }
        else
        {
            // Position reached: Hover up and down slowly using a Sine wave
            float hoverY = Mathf.Sin(Time.time * 2f) * speed;
            rb.linearVelocity = new Vector2(0, hoverY);
        }
    }

    ///<summary>
    ///Infinite loop picking a random attack from M1, M2, M5, or M6 every 4 seconds.
    ///</summary>
    private IEnumerator BossAttackCycle()
    {
        // Give the boss 2 seconds to enter the screen before starting to attack
        yield return new WaitForSeconds(2f);

        while (IsAlive)
        {
            currentAttackPhase = Random.Range(0, 4); // Picks 0, 1, 2, or 3

            switch (currentAttackPhase)
            {
                case 0:
                    Debug.Log("Boss Attack: M1 Electric Ball!");
                    Shoot(electricPrefab);
                    break;

                case 1:
                    Debug.Log("Boss Attack: M2 Flame Cone Burst!");
                    // Fires 2 quick bursts of flame cones
                    for (int i = 0; i < 2; i++)
                    {
                        Shoot(flamePrefab);
                        yield return new WaitForSeconds(0.3f);
                    }
                    break;

                case 2:
                    Debug.Log("Boss Attack: M5 Giant Horizontal Laser!");
                    Shoot(laserPrefab); // Spawns the laser beam prefab that stays 2s
                    break;

                case 3:
                    Debug.Log("Boss Attack: M6 Sniper Burst!");
                    // Fires a rapid burst of 4 sniper bullets
                    for (int i = 0; i < 4; i++)
                    {
                        Shoot(sniperPrefab);
                        yield return new WaitForSeconds(0.12f);
                    }
                    break;
            }

            // Wait 4 seconds before the next attack pattern
            yield return new WaitForSeconds(4f);
        }
    }
}