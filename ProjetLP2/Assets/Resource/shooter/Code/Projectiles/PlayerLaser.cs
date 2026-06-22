using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] private float laserSpeed = 12f;
    [SerializeField] private float damageValue = 1f;
    [SerializeField] private float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * laserSpeed * Time.deltaTime);
    }

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