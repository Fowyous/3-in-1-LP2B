using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 12f;
    [SerializeField] private float damageValue = 1f; 
    [SerializeField] private float lifeTime = 3f;    

    ///<summary>
    ///Schedules the destruction of the laser instance to prevent memory leaks.
    ///</summary>
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    ///<summary>
    ///Moves the laser horizontally across the screen.
    ///</summary>
    void Update()
    {
        transform.Translate(Vector3.right * laserSpeed * Time.deltaTime);
    }

    ///<summary>
    ///Detects collision with enemies, applies damage via IEnemy, and destroys the projectile.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IEnemy enemy = collision.GetComponent<IEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damageValue);
            Destroy(gameObject); 
        }
    }
}