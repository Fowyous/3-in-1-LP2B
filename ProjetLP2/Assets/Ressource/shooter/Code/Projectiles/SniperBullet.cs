using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [Header("Sniper Stats")]
    [SerializeField] private float speed = 16f;          // Very high velocity
    [SerializeField] private float damage = 0.25f;       // 0.25 damage per bullet 
    [SerializeField] private float lifeTime = 3f;

    ///<summary>
    ///Schedules bullet cleanup.
    ///</summary>
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    ///<summary>
    ///Moves the sniper bullet rapidly to the left.
    ///</summary>
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    ///<summary>
    ///Applies minor but rapid damage to the UFO upon impact.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
