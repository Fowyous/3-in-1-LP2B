using UnityEngine;
///<summary>
///enemy interface.
///enemy can take damage or shoot 
///so a shoot and take damage method is imposed
///an IsAlive boolean is necessary
public interface IEnemy
{
  float Health { get; set; }
  float Damage { get; set; }
  void TakeDamage(float damage);
  void Shoot(GameObject bullet);
  bool IsAlive { get; }
}
