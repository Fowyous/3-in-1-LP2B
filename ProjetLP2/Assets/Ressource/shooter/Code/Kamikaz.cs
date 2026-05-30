using UnityEngine;
///<summary>
///the Kamikaz enemy, charges at the player then explodes on touch.
///</summary>
public class Kamikaz : MonoBehaviour, IEnemy
{
  public float Health { get; set; } = 1;
  public float Damage { get; set; } = 5;
  public bool IsAlive { get; } = true;
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


}
