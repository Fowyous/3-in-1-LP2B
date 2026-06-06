using UnityEngine;
using static ShooterConstants;

///<summary>
///the Kamikaz enemy, charges at the player then explodes on touch.
///</summary>
public class Kamikaz : MonoBehaviour, IEnemy
{
  public float Health { get; set; } = 1;
  public float Damage { get; set; } = 5;
  public bool IsAlive { get; private set; } = true;
  private float SPEED { get; set; } = 7.0f;
  private Vector3 moveDirection = Vector3.left;
  private Vector3 lastDirection;

  //the target is set externally by the spawner
  public GameObject Target { get; set; }
  private Rigidbody2D rb;
  void Start()
  {
    Debug.Log("Kamikaz spawned");
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

  void Update()
  {

    NextMove(Target);
  }
  public void TakeDamage(float damage)
  {
    Health -= damage;
    Debug.Log($"Kamikaz took {damage} damage. Health: {Health}");
  }


  ///<summary>
  ///Here the enemy has to explode on the player when the distance is close enough.
  ///This method gets called when we detect that we are too close to the player.
  ///</summary>
  public void Shoot(GameObject bullet)
  {
  }

  ///Determines and applies the next move of the enemy Kamikaz.
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

    Vector3 toTarget = (target.transform.position - transform.position).normalized;

    // phase boundaries 
    float p1 = ShooterConstants.Phase1limit;
    float p2 = ShooterConstants.Phase2limit;

    // Determine move direction per phase:
    // - Phase 1: continue straight along current forward
    // - Phase 2: follow the player
    // - After Phase 2: stop following and keep last direction
    if (transform.position.x >= p1)
    {
      Debug.Log("p1");
      moveDirection = Vector3.left;
      lastDirection = moveDirection;
    }
    else if (transform.position.x < p1 && transform.position.x >= p2)
    {
      Debug.Log("p2");
      moveDirection = toTarget;
      lastDirection = moveDirection;
    }
    else
    {
      Debug.Log("p3");
      // keep whatever direction we had at end of phase 2 (if that direction is not 0)
      moveDirection = lastDirection != Vector3.zero ? lastDirection : transform.forward;
    }
    // apply velocity for the actual direction of the enemy.
    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;


  }


}
