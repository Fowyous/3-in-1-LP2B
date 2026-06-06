using UnityEngine;

///<summary>
///The Horizontal Laser projectile fired by M5. It extends across the screen from the FirePoint.
///</summary>
public class HorizontalLaser : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float damagePerSecond = 2f;
    [SerializeField] private float duration = 2f; 
    [SerializeField] private float laserLength = 25f;

    private LineRenderer lineRenderer;

    ///<summary>
    ///Initializes the component and schedules auto-destruction.
    ///</summary>
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Destroy(gameObject, duration); // Automatically cleans up the object after its lifespan
    }

    ///<summary>
    ///Ancors the ray origin point dynamically and executes physics raycast damage checks.
    ///</summary>
    void Update()
    {
        if (lineRenderer == null) return;

        // Visual fix: locks point 0 directly on the current firepoint tracking position
        lineRenderer.SetPosition(0, transform.position);

        Vector2 direction = Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, laserLength);

        if (hit.collider != null)
        {
            // Point 1 snaps dynamically to the surface of whatever object it collides with
            lineRenderer.SetPosition(1, hit.point);

            // Apply ticking damage if the hit object is the player UFO
            UFO player = hit.collider.GetComponent<UFO>();
            if (player != null)
            {
                player.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            // If the ray hits absolutely nothing, it stretches out entirely to the left edge
            lineRenderer.SetPosition(1, transform.position + Vector3.left * laserLength);
        }
    }
}