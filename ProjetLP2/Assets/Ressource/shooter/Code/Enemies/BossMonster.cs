using UnityEngine;
using System.Collections;

public class BossMonster : MonoBehaviour, IEnemy
{
    [Header("Boss Stats")]
    [SerializeField] private float health = 15f;    // 15 HP
    [SerializeField] private float damage = 5f;
    [SerializeField] private float speed = 1.5f;     // Slow speed

    [Header("Boss Projectiles / Prefabs")]
    [SerializeField] private GameObject electricPrefab; // M1
    [SerializeField] private GameObject sniperPrefab;   // M6
    [SerializeField] private GameObject kamikazPrefab;  // M4 minion spawn

    public float Health { get => health; set => health = value; }
    public float Damage { get => damage; set => damage = value; }
    public bool IsAlive { get; private set; } = true;
    public GameObject Target { get; set; }

    private Rigidbody2D rb;
    private int currentAttackPhase = 0;

    ///<summary>
    ///Starts the periodic boss attack sequence routine.
    ///</summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(BossAttackCycle());
    }

    ///<summary>
    ///Moves the boss towards the left play area.
    ///</summary>
    void Update()
    {
        NextMove(Target);
    }

    ///<summary>
    ///Cycles through different abilities every 4 seconds.
    ///</summary>
    private IEnumerator BossAttackCycle()
    {
        while (IsAlive)
        {
            yield return new WaitForSeconds(4f);
            currentAttackPhase = Random.Range(0, 3); // Randomly choose an attack pattern

            switch (currentAttackPhase)
            {
                case 0:
                    Shoot(electricPrefab); // Cast electric attack
                    break;
                case 1:
                    // Spawn a small Kamikaze minion to rush the player
                    if (kamikazPrefab != null)
                    {
                        GameObject minion = Instantiate(kamikazPrefab, transform.position, Quaternion.identity);
                        minion.GetComponent<IEnemy>().Target = Target;
                    }
                    break;
                case 2:
                    // Rapid fire outburst
                    Shoot(sniperPrefab);
                    break;
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
        if (bullet != null)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }

    public void NextMove(GameObject target)
    {
        if (rb == null) return;
        // The boss moves slowly forward into Zone 2 and hovers around
        if (transform.position.x > 2f)
        {
            rb.linearVelocity = Vector3.left * speed;
        }
        else
        {
            // Hover up and down slowly in place
            rb.linearVelocity = new Vector2(0, Mathf.Sin(Time.time) * speed);
        }
    }
}
