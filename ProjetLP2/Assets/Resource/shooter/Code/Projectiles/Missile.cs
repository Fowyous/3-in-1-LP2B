using UnityEngine;

public class Missile : MonoBehaviour
{
  [SerializeField] private float missileSpeed = 12f;
  [SerializeField] private float damage = 10f;
  [SerializeField] private float lifeTime = 3f;
  ///<summary>
  ///Schedules the destruction of the laser instance to prevent memory leaks.
  ///</summary>
  void Start()
  {
    Destroy(gameObject, lifeTime);

  }


  ///<summary>
  ///Moves the missile horizontally across the screen.
  ///</summary>
  void Update()
  {
    transform.Translate(Vector3.left * missileSpeed * Time.deltaTime);

  }

  ///<summary>
  ///Detects collision with the UFO (damage) or the Base (damage only), then self-destructs.
  ///</summary>
  private void OnTriggerEnter2D(Collider2D collision)
  {
    UFO player = collision.GetComponent<UFO>();
    if (player != null)
    {
      player.TakeDamage(damage);
      //      player.ApplyStun(stunDuration);
      Debug.Log($"Missile: {damage} damage applied to the UFO.");
      Destroy(gameObject);
      return;
    }

    if (collision.CompareTag("Base"))
    {
      BaseManager baseScript = collision.GetComponent<BaseManager>();
      if (baseScript != null)
      {
        baseScript.TakeDamage(damage);
        Debug.Log($"Missile : {damage} damage to the Base.");
      }
      Destroy(gameObject);
    }
  }


}
