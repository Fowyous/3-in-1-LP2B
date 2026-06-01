using UnityEngine;
using static ShooterConstants;

///<summary>
///The EnergyBallThrower enemy, keeps a safe distance from the player while shooting an electric charge at them.
///</summary>
public class EnergyBallThrower : MonoBehaviour, IEnemy
{
  public float Health { get; set; } = 1;
  public float Damage { get; set; } = 5;
  public bool IsAlive { get; private set; } = true;
  private float SPEED { get; set; } = 3.0f;
  private Vector3 moveDirection = Vector3.left;
  private Vector3 lastDirection;
  private bool intro = true;
  private static float introDuration = 2.0f;
  private float introProgress;
  private Vector2 startPos;

  public GameObject Target { get; set; }
  private Rigidbody2D rb;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    startPos = transform.position;
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

  public void TakeDamage(float damage)
  {
    Health -= damage;
    Debug.Log($"Kamikaz took {damage} damage. Health: {Health}");
  }

  ///<summary>
  ///Here the enemy has to shoot an EnergyBall
  ///This method gets called every specified period of time
  ///</summary>
  public void Shoot(GameObject bullet)
  {
  }

  // Update is called once per frame
  void Update()
  {
    NextMove(Target);


  }
  ///<summary>
  ///The Energyball thrower moves into phase one and stays in it.
  ///they follow the player on the horizontal axis playery = EnergyBallThrowery
  ///they stop and lock on the player for 1 second then
  ///shoot immediately.
  ///</summary>
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

    float positionY = ShooterConstants.Phase1limit + 1;

    //the enemy introduces themselves on the scene.(fixed y axis moving x axis)
    if (intro)
    {
      introProgress += Time.deltaTime / introDuration;
      transform.position = Vector2.Lerp(
          startPos,
          new Vector2(positionY, transform.position.y),
          introProgress
          );
      if (introProgress >= 1f)
      {
        intro = false;
        moveDirection = Vector3.zero;

      }

    }
    else//the enemy follows the player. (fixed x axis moving y axis)
    {
      float targetY = target.transform.position.y;
      float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

      // Only move if not at target Y position 
      if (Mathf.Abs(targetY - transform.position.y) > 0.1f)
      {
        float directionY = Mathf.Sign(targetY - transform.position.y);
        moveDirection = new Vector3(0, directionY, 0);
      }
      else
      {
        moveDirection = Vector3.zero;
        //if we are at target Y position for long enough (1 second i think)
        //we should lock the position and shoot (the stop should be around 1 second)
        // TODO: Add shooting logic here after a small wai
      }
    }

    // apply velocity.
    rb.linearVelocity = new Vector2(moveDirection.x, moveDirection.y).normalized * SPEED;


  }
}
