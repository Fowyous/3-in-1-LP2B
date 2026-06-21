using UnityEngine;

public class FlameCone : MonoBehaviour
{
    [Header("Flame Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float directDamage = 1f;
    [SerializeField] private float burnDamagePerSec = 0.5f;
    [SerializeField] private float burnDuration = 4f;
    [SerializeField] private float expansionSpeed = 1.5f;

    private float lifeTime = 1.2f;
    private GameObject _target;

    public void SetTarget(GameObject target)
    {
        _target = target;
        float distanceToLeftEdge = transform.position.x - (-8f);
        lifeTime = Mathf.Max(1.2f, distanceToLeftEdge / speed);
        Destroy(gameObject, lifeTime);
    }

    void Start()
    {
        Invoke(nameof(FallbackDestroy), lifeTime);
    }

    private void FallbackDestroy()
    {
        if (gameObject != null) Destroy(gameObject);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        transform.localScale += new Vector3(0, expansionSpeed * Time.deltaTime, 0);
    }

    ///<summary>
    ///Applies direct damage + burn to the UFO, or direct damage only to the Base.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(directDamage);
            player.ApplyBurn(burnDamagePerSec, burnDuration);
            Debug.Log($"FlameCone: {directDamage} dégâts directs + brûlure appliqués à l'UFO.");
            Destroy(gameObject);
            return;
        }

        if (collision.CompareTag("Base"))
        {
            BaseManager baseScript = collision.GetComponent<BaseManager>();
            if (baseScript != null)
            {
                baseScript.TakeDamage(directDamage);
                Debug.Log($"FlameCone: {directDamage} dégâts infligés à la Base.");
            }
            Destroy(gameObject);
        }
    }
}