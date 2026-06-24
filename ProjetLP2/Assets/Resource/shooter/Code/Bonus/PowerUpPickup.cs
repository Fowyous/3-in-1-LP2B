using UnityEngine;

///<summary>
///A power-up pickup that drops from a dead enemy and drifts left across the screen,
///exactly like other projectiles. When the UFO touches it, the corresponding
///effect is applied and the pickup is destroyed. If never picked up, it is
///destroyed automatically once it exits the screen on the left, to avoid
///wasting memory on pickups nobody ever collects.
///</summary>
public class PowerUpPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private PowerUpType type;
    [SerializeField] private float speed = 4f;

    [Header("Cleanup")]
    [Tooltip("X position past which the pickup is destroyed if never collected.")]
    [SerializeField] private float destroyXThreshold = -10f;

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Cleanup: destroy once it has fully exited the screen on the left, uncollected
        if (transform.position.x < destroyXThreshold)
        {
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Applies the power-up effect to the UFO on contact, then destroys the pickup.
    ///</summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UFO player = collision.GetComponent<UFO>();
        if (player == null) return;

        player.ApplyPowerUp(type);
        Debug.Log($"PowerUpPickup: {type} collected by the UFO.");
        Destroy(gameObject);
    }
}