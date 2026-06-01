using UnityEngine;
using static ShooterConstants;

///<summary>
///The EnergyBallThrower enemy, keeps a safe distance from the player while shooting an electric charge at them.
///</summary>
public class EnergyBallThrower : MonoBehaviour
{
  public float Health { get; set; } = 1;
  public float Damage { get; set; } = 5;
  public bool IsAlive { get; private set; } = true;
  private float SPEED { get; set; } = 7.0f;
  private Vector3 moveDirection = Vector3.left;
  private Vector3 lastDirection;

  public GameObject Target { get; set; }
  private Rigidbody2D rb;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
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
  public void NextMove(GameObject target)
  {
  }
}
