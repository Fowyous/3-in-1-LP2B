using UnityEngine;
using ShooterConstants;
///<summary>
///the Kamikaz enemy, charges at the player then explodes on touch.
///</summary>
public class Kamikaz : MonoBehaviour, IEnemy
{
  public float Health { get; set; } = 1;
  public float Damage { get; set; } = 5;
  public bool IsAlive { get; } = true;
  static float SPEED { get; set; } = 7.0f;

  public Rigidbody rb;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

  }
  public void TakeDamage(float damage)
  {
    Health -= damage;
    Debug.Log($"Kamikaz took {damage} damage. Health: {Health}");
  }


  public void Shoot(GameObject bullet)
  {
  }

  ///<summary>
  ///Here the Kamikaz enemy will continue in a straight line through phase 1 and then will follow the player during all of phase 2 then will 
  ///stop following and keep the same direction.
  ///</summary>
  public void NextMove(GameObject Target)
  {
    if (transform.position.x <= ShooterConstants.Phase1limit)
    {

      // we use moveDirection.normalized so that the speed doesn't change when we go diagonally
      rb.velocity = transform.TransformDirection(moveDirection.normalized) * SPEED * Time.Deltatime;


    }

  }


}
