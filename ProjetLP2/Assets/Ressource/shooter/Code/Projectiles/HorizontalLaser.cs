using UnityEngine;

///<summary>
///The Horizontal Laser projectile fired by M5 (and the Boss). It extends across the screen
///from the FirePoint and applies continuous damage to whatever it touches: UFO or Base.
///Uses a BoxCollider2D (Trigger) stretched to match the visual laser length,
///exactly like the other projectiles (ElectricBall, FlameCone, etc.).
///If a source Transform is assigned, the laser follows it every frame so it stays
///anchored to its shooter even if the shooter keeps moving (e.g. the Boss hovering).
///</summary>
[RequireComponent(typeof(BoxCollider2D))]
public class HorizontalLaser : MonoBehaviour
{
  [Header("Laser Settings")]
  [SerializeField] private float damagePerSecond = 2f;
  [SerializeField] private float duration = 2f;
  [SerializeField] private float laserLength = 30f;
  [SerializeField] private float laserThickness = 0.3f; // Visual + collision height of the beam

  private LineRenderer lineRenderer;
  private BoxCollider2D boxCollider;
  private Transform sourcePoint;

  void Start()
  {
    lineRenderer = GetComponent<LineRenderer>();
    boxCollider = GetComponent<BoxCollider2D>();
    boxCollider.isTrigger = true;

    Destroy(gameObject, duration);
  }

  ///<summary>
  ///Called by the shooter (M5 or Boss) right after instantiating the laser,
  ///so it can re-anchor to the shooter's current position every frame.
  ///</summary>
  public void SetSource(Transform source)
  {
    sourcePoint = source;
  }

  void Update()
  {
    // Re-anchor to the shooter's current position every frame, if assigned
    if (sourcePoint != null)
    {
      transform.position = sourcePoint.position;
      transform.rotation = sourcePoint.rotation;
    }

    // Update the visual line: from the fire point, stretching left across the screen
    if (lineRenderer != null)
    {
      lineRenderer.SetPosition(0, transform.position);
      lineRenderer.SetPosition(1, transform.position + Vector3.left * laserLength);
    }

    // Update the collider to match the visual laser:
    // it must span from the fire point to laserLength on the left,
    // so its center is offset by half the length, and centered locally on this transform.
    if (boxCollider != null)
    {
      boxCollider.size = new Vector2(laserLength, laserThickness);
      boxCollider.offset = new Vector2(-laserLength / 2f, 0f);
    }
  }

  ///<summary>
  ///Applies continuous damage to whatever enters the laser's collider: UFO or Base.
  ///Damage ticks every frame the target stays inside the beam.
  ///</summary>
  private void OnTriggerStay2D(Collider2D collision)
  {
    UFO player = collision.GetComponent<UFO>();
    if (player != null)
    {
      player.TakeDamage(damagePerSecond * Time.deltaTime);
      return;
    }

    BaseManager baseScript = collision.GetComponent<BaseManager>();
    if (baseScript != null)
    {
      baseScript.TakeDamage(damagePerSecond * Time.deltaTime);
    }
  }
}
