using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float damage = 2f;         // 2 damage as specified for M1
    [SerializeField] private float stunDuration = 2f;   // 2s immobilization
    [SerializeField] private float lifeTime = 4f;

    ///<summary>
    ///Schedules destruction of the electric ball to save memory.
    ///</summary>
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    ///<summary>
    ///Moves the electric ball straight to the left.
    ///</summary>
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    ///<summary>
    ///Detects collision with the UFO, applies damage and stun, then destroys the projectile.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(damage);
            player.ApplyStun(stunDuration);
            Debug.Log($"ElectricBall: {damage} dégâts + stun {stunDuration}s appliqué.");
            Destroy(gameObject);
        }
    }
}