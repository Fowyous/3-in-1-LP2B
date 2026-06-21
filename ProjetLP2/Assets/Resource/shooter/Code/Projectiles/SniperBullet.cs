using UnityEngine;

public class SniperBullet : MonoBehaviour
{
    [Header("Sniper Stats")]
    [SerializeField] private float speed = 16f;
    [SerializeField] private float damage = 0.25f;
    [SerializeField] private float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    ///<summary>
    ///Applies damage to whichever target it hits: the UFO or the Base.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
                baseScript.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}