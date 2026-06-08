using UnityEngine;
using System.Collections;

///<summary>
///Manages the UFO respawn cinematic.
///Handles explosion VFX, repositioning, health reset, and invincibility frames.
///Call TriggerRespawn() from UFO.Die() to start the sequence.
///</summary>
public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance { get; private set; }

    [Header("Respawn Settings")]
    [Tooltip("Fixed position where the UFO reappears after death.")]
    [SerializeField] private Vector3 respawnPosition = new Vector3(-6f, 0f, 0f);
    [SerializeField] private float respawnDelay = 2f;       // Seconds before the UFO reappears
    [SerializeField] private float invincibilityDuration = 2f; // Seconds of invincibility after respawn
    [SerializeField] private float blinkInterval = 0.15f;   // Blink speed during invincibility

    [Header("Explosion VFX")]
    [Tooltip("Drag your explosion sprite/prefab here.")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionDuration = 1f;  // How long the explosion stays on screen

    // Set to false by BaseManager when the base is destroyed
    public bool CanRespawn { get; set; } = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple RespawnManager instances detected!");
            Destroy(gameObject);
        }
    }

    ///<summary>
    ///Entry point called by UFO.Die().
    ///Checks if respawn is allowed, then starts the cinematic coroutine.
    ///</summary>
    public void TriggerRespawn(UFO player)
    {
        if (!CanRespawn)
        {
            // Base is destroyed: no more respawns, trigger game over instead
            Debug.Log("RespawnManager: No respawn available. Base is destroyed. GAME OVER.");
            if (GameOverManager.Instance != null)
                GameOverManager.Instance.TriggerGameOver();
            return;
        }

        StartCoroutine(RespawnSequence(player));
    }

    ///<summary>
    ///Full respawn cinematic:
    ///1. Spawn explosion VFX at death position
    ///2. Hide the UFO and block inputs
    ///3. Wait for respawn delay
    ///4. Reposition and restore health
    ///5. Apply invincibility frames with blink effect
    ///</summary>
    private IEnumerator RespawnSequence(UFO player)
    {
        Debug.Log("RespawnManager: Respawn sequence started.");

        // Step 1: Spawn explosion at the UFO's current position
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, player.transform.position, Quaternion.identity);
            Destroy(explosion, explosionDuration);
        }

        // Step 2: Hide the UFO and disable its controls
        player.SetActive(false);

        // Step 3: Wait before reappearing (game continues, enemies keep moving)
        yield return new WaitForSeconds(respawnDelay);

        // Step 4: Reposition UFO to the fixed respawn point and restore full health
        player.transform.position = respawnPosition;
        player.RestoreHealth();
        player.SetActive(true);

        Debug.Log($"RespawnManager: UFO respawned at {respawnPosition}.");

        // Step 5: Apply invincibility frames with a blink effect
        StartCoroutine(InvincibilityFrames(player));
    }

    ///<summary>
    ///Makes the UFO invincible and blinks its sprite for the invincibility duration.
    ///</summary>
    private IEnumerator InvincibilityFrames(UFO player)
    {
        player.SetInvincible(true);

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        float elapsed = 0f;

        while (elapsed < invincibilityDuration)
        {
            if (sr != null) sr.enabled = !sr.enabled; // Toggle visibility for blink effect
            elapsed += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        // Make sure the sprite is visible when invincibility ends
        if (sr != null) sr.enabled = true;
        player.SetInvincible(false);

        Debug.Log("RespawnManager: Invincibility ended.");
    }
}