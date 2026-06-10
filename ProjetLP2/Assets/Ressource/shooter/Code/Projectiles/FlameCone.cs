using UnityEngine;

public class FlameCone : MonoBehaviour
{
    [Header("Flame Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float directDamage = 1f;
    [SerializeField] private float burnDamagePerSec = 0.5f;
    [SerializeField] private float burnDuration = 4f;
    [SerializeField] private float expansionSpeed = 1.5f;

    // Lifetime is now computed dynamically based on distance to travel
    private float lifeTime = 1.2f;
    private GameObject _target;

    ///<summary>
    ///Called by FlameThrower.Shoot() to inject the target reference.
    ///Adjusts lifetime so the flame can always reach the left edge of the screen.
    ///</summary>
    public void SetTarget(GameObject target)
    {
        _target = target;

        // Calculate the distance from spawn position to the left edge of the screen (~-8f)
        float distanceToLeftEdge = transform.position.x - (-8f);

        // Compute lifetime so the flame travels at least that far
        lifeTime = Mathf.Max(1.2f, distanceToLeftEdge / speed);

        // Schedule destruction with the adjusted lifetime
        Destroy(gameObject, lifeTime);
    }

    void Start()
    {
        // Fallback: if SetTarget was never called, use the default short lifetime
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player != null)
        {
            player.TakeDamage(directDamage);
            player.ApplyBurn(burnDamagePerSec, burnDuration);
            Debug.Log($"FlameCone: {directDamage} dégâts directs + brûlure {burnDamagePerSec}/s pendant {burnDuration}s.");
            Destroy(gameObject);
        }
    }
}