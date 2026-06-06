using UnityEngine;

///<summary>
///The Horizontal Laser projectile fired by M5. It extends across the screen and damages the player.
///</summary>
public class HorizontalLaser : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float damagePerSecond = 2f;
    [SerializeField] private float duration = 2f; // How long the laser stays on screen
    [SerializeField] private float laserLength = 25f;

    private LineRenderer lineRenderer;

    ///<summary>
    ///Initializes the LineRenderer and schedules the laser's destruction.
    ///</summary>
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Destroy(gameObject, duration); // Automatically cleans up the laser after 'duration' seconds
    }

    ///<summary>
    ///Constantly projects the laser beam and checks for hit collisions with the UFO player.
    ///</summary>
    void Update()
    {
        if (lineRenderer == null) return;

        // Point 0 starts at the fire point position
        lineRenderer.SetPosition(0, transform.position);

        Vector2 direction = Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, laserLength);

        if (hit.collider != null)
        {
            // Point 1 stops where it hits an object
            lineRenderer.SetPosition(1, hit.point);

            // If it hits the UFO, apply continuous damage
            UFO player = hit.collider.GetComponent<UFO>();
            if (player != null)
            {
                player.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            // If it hits nothing, stretch all the way to the left
            lineRenderer.SetPosition(1, transform.position + Vector3.left * laserLength);
        }
    }
}
