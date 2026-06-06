using UnityEngine;
using static ShooterConstants;
///<summary>
///The flamethrower enemy. We don't have a flame prefab for the moment so i think we should improvise with the enemy shot 1 prefab.
///the beam should disappear at short distance and the damage should last a bit (an effect of burn).
///Movement: The enemy should follow the player from their front and start shooting when they get in range to shoot.
///</summary>
public class FlameThrower : MonoBehaviour, IEnemy
{
  [Header("Enemy main variables")]
  [SerializeField]
  private float _health = 1f;  // Backing field (serialized)

  [SerializeField]
  private float _damage = 5f;

  [SerializeField]
  private float _speed = 3.5f;

  [SerializeField]
  private GameObject _target;
  [SerializeField] private GameObject flameConePrefab;
[SerializeField] private Transform firePoint;
private float nextFireTime = 0f;
private float fireRate = 1.5f;

  private bool _isAlive = true;  // Private backing field (NOT serialized)

  [Space(10)]
  [Header("FlameThrower specific variables")]
  [Tooltip("The distance to maintain to the right of the player")]
  private float distancetomaintain = 2.0f;

  private bool intro = true;

  public float Health
  {
    get => _health;
    set => _health = value;
  }

  public float Damage
  {
    get => _damage;
    set => _damage = value;
  }

  public float SPEED
  {
    get => _speed;
    set => _speed = value;
  }

  public GameObject Target
  {
    get => _target;
    set => _target = value;
  }
  private Vector3 moveDirection = Vector3.left;
  private Vector3 lastDirection;

  public bool IsAlive { get; private set; } = true;
  private Rigidbody2D rb;

  void Start()
  {
    Debug.Log("FlameThrower spawned");
    rb = GetComponent<Rigidbody2D>();
    //Verify rb was found  
    if (rb == null)
    {
      Debug.LogError("RIGIDBODY NOT FOUND! Make sure this GameObject has a Rigidbody component.");
    }
    else
    {
      Debug.Log("Rigidbody successfully assigned");
    }

  }

  // Update is called once per frame
  void Update()
  {
    NextMove(Target);
    if (!intro && Vector3.Distance(transform.position, Target.transform.position) <= 4.0f)
{
    if (Time.time >= nextFireTime)
    {
        Shoot(flameConePrefab);
        nextFireTime = Time.time + fireRate;
    }
}


  }

  public void TakeDamage(float damage)
  {
    Health -= damage;
    Debug.Log($"Kamikaz took {damage} damage. Health: {Health}");
  }

///<summary>
///This method gets called when we detect that we are too close to the player.
///Instantiates the flame cone projectile at the fire point.
///</summary>
public void Shoot(GameObject bullet)
{
    if (bullet != null && firePoint != null)
    {
        Instantiate(bullet, firePoint.position, firePoint.rotation);
    }
}

  ///Determines and applies the next move of the enemy Flame Thrower.
  public void NextMove(GameObject target)
  {
    //NULL checks
    if (rb == null || target == null)
    {
      Debug.LogError("null check failed");
      Debug.LogError("rb : " + rb);
      Debug.LogError("target : " + target);
      return;
    }

    Vector3 toTarget = (target.transform.position - transform.position);

    //the target that the enemy should go to is right in front of the player and not on top
    toTarget.x += distancetomaintain;

    //since the shooting range is large (but short) we try to not be directly in front of the player to make things harder.
    toTarget.y += Random.Range(-1.0f, 1.0f);

    // Normalize AFTER making adjustments
    toTarget = toTarget.normalized;


    // phase boundaries 
    float p1 = ShooterConstants.Phase1limit;
    float p2 = ShooterConstants.Phase2limit;

    // Determine move direction:
    // - intro: continue straight along current forward
    // - attack:
    //    - Step 1 : follow the player while staying in front and maintaining distance(if player in front move forward/ if player behind move back)
    //    - Step 2 : if the distance is good start shooting (a bit behind the player or in front)
    //    -
    if (transform.position.x >= p1 && intro)
    {
      Debug.Log("intro");
      moveDirection = Vector3.left;
      lastDirection = moveDirection;
    }
    else if (transform.position.x < p1 && intro)
    {
      intro = false;
      Debug.Log("intro done");
    }
    else
    {// if(!intro) : move to target
      Debug.Log("moving to target");
      moveDirection = toTarget;
      lastDirection = moveDirection;

    }
    // apply velocity for the actual direction of the enemy.
    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;


  }

}
