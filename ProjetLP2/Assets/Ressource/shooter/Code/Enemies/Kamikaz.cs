using UnityEngine;
using static ShooterConstants;

///<summary>
///The Kamikaz enemy, charges at the player then explodes on touch.
///</summary>
public class Kamikaz : MonoBehaviour, IEnemy
{
    public float Health { get; set; } = 1;
    public float Damage { get; set; } = 5;
    public bool IsAlive { get; private set; } = true;
    private float SPEED { get; set; } = 7.0f;
    private Vector3 moveDirection = Vector3.left;
    private Vector3 lastDirection;

    // The target is set externally by the spawner
    public GameObject Target { get; set; }
    private Rigidbody2D rb;

    ///<summary>
    ///Initializes components and verifies the Rigidbody2D reference.
    ///</summary>
    void Start()
    {
        Debug.Log("Kamikaz spawned");
        rb = GetComponent<Rigidbody2D>();
        
        if (rb == null)
        {
            Debug.LogError("RIGIDBODY NOT FOUND! Make sure this GameObject has a Rigidbody component.");
        }
    }

    ///<summary>
    ///Updates the movement vector calculations and physics application on every frame.
    ///</summary>
    void Update()
    {
        NextMove(Target);
    }

    ///<summary>
    ///Applies damage to the Kamikaz and destroys it if health drops below or equal to zero.
    ///</summary>
    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        Health -= damage;
        Debug.Log($"Kamikaz took {damage} damage. Health: {Health}");

        if (Health <= 0)
        {
            IsAlive = false;
            Debug.Log("Kamikaz destroyed by player shot.");
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Required by IEnemy interface contract. Intentionally left blank as Kamikaz acts as its own projectile.
    ///</summary>
    public void Shoot(GameObject bullet)
    {
        // No ranged projectile shooting logic required for this unit type
    }

    ///<summary>
    ///Determines and applies the phase-based movement vector towards the player target.
    ///</summary>
    public void NextMove(GameObject target)
    {
        if (rb == null || target == null) return;

        Vector3 toTarget = (target.transform.position - transform.position).normalized;

        float p1 = ShooterConstants.Phase1limit;
        float p2 = ShooterConstants.Phase2limit;

        if (transform.position.x >= p1)
        {
            moveDirection = Vector3.left;
            lastDirection = moveDirection;
        }
        else if (transform.position.x < p1 && transform.position.x >= p2)
        {
            moveDirection = toTarget;
            lastDirection = moveDirection;
        }
        else
        {
            moveDirection = lastDirection != Vector3.zero ? lastDirection : Vector3.left;
        }

        rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;
    }

    ///<summary>
    ///Detects proximity and impact with the UFO player, triggers instant detonation and inflicts structural damage.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(Damage); // Deals 5 damage
            Debug.Log("Kamikaz exploded on the player!");
            Destroy(gameObject); // Destroys itself upon detonation
        }
    }
}