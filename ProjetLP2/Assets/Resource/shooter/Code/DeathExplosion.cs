using UnityEngine;

///<summary>
///Add this script to any enemy prefab.
///When the enemy is destroyed, it instantiates an explosion prefab at its position.
///The explosion destroys itself automatically after its animation finishes.
///</summary>
public class DeathExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [Tooltip("Drag your explosion prefab here (ExplosionRed or ExplosionGreen).")]
    [SerializeField] private GameObject explosionPrefab;

    [Tooltip("How long the explosion stays on screen before being destroyed.")]
    [SerializeField] private float explosionDuration = 1f;

    ///<summary>
    ///Called automatically by Unity when this GameObject is destroyed.
    ///Spawns the explosion VFX at the monster's last position.
    ///</summary>
    private void OnDestroy()
    {
        // Avoid spawning explosion when the scene is unloaded (e.g. game restart)
        if (!gameObject.scene.isLoaded) return;

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );
            Destroy(explosion, explosionDuration);
        }
        else
        {
            Debug.LogWarning($"DeathExplosion on '{gameObject.name}': explosionPrefab is not assigned!");
        }
    }
}