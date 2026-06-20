using UnityEngine;

public class ElectricBall : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private float lifeTime = 4f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    ///<summary>
    ///Detects collision with the UFO (damage + stun) or the Base (damage only), then self-destructs.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(damage);
            player.ApplyStun(stunDuration);
            Debug.Log($"ElectricBall: {damage} dégâts + stun {stunDuration}s appliqué à l'UFO.");
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
            {
                baseScript.TakeDamage(damage);
                Debug.Log($"ElectricBall: {damage} dégâts infligés à la Base.");
            }
            Destroy(gameObject);
        }
    }
}