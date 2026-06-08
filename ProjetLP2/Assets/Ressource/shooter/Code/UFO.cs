using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using static ShooterConstants;

public class UFO : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    [Header("Combat Settings")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float maxHealth = 10f;

    public float CurrentHealth { get; private set; }

    private float nextFireTime = 0f;
    private Rigidbody2D rb;

    // Status flags
    private bool _isStunned = false;
    private bool _isBurning = false;
    private bool _isInvincible = false;
    private bool _isActive = true; // False while the respawn cinematic is playing

    ///<summary>
    ///Initializes player health and caches the Rigidbody2D component.
    ///</summary>
    void Start()
    {
        CurrentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("UFO: Rigidbody2D component missing on this GameObject!");
        }
    }

    ///<summary>
    ///Frame-rate independent updates for movement and shooting.
    ///Skipped entirely while the UFO is inactive (respawn cinematic).
    ///</summary>
    void Update()
    {
        if (!_isActive) return;
        HandleMovement();
        HandleShooting();
    }

    ///<summary>
    ///Handles player movement with keyboard input and restricts position within ShooterConstants limits.
    ///Movement is blocked while the player is stunned.
    ///</summary>
    private void HandleMovement()
    {
        if (_isStunned) return;

        Vector3 moveDirection = Vector3.zero;
        if (Keyboard.current.rightArrowKey.isPressed) moveDirection.x = 1;
        if (Keyboard.current.leftArrowKey.isPressed) moveDirection.x = -1;
        if (Keyboard.current.upArrowKey.isPressed) moveDirection.y = 1;
        if (Keyboard.current.downArrowKey.isPressed) moveDirection.y = -1;

        transform.Translate(moveDirection * Time.deltaTime * speed);

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -ShooterConstants.GameLimit.x, ShooterConstants.GameLimit.x);
        pos.y = Mathf.Clamp(pos.y, -ShooterConstants.GameLimit.y, ShooterConstants.GameLimit.y);
        transform.position = pos;
    }

    ///<summary>
    ///Monitors the spacebar input and triggers weapon firing if the cooldown timer has elapsed.
    ///</summary>
    private void HandleShooting()
    {
        if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
        {
            ShootLaser();
            nextFireTime = Time.time + fireRate;
        }
    }

    ///<summary>
    ///Instantiates a laser projectile at the designated fire point position.
    ///</summary>
    private void ShootLaser()
    {
        if (laserPrefab != null && firePoint != null)
        {
            Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        }
        else
        {
            Debug.LogWarning("UFO: Laser Prefab or Fire Point reference missing in Inspector.");
        }
    }

    ///<summary>
    ///Inflicts damage to the player. Ignored if invincible or inactive.
    ///</summary>
    public void TakeDamage(float damageAmount)
    {
        if (_isInvincible || !_isActive) return;

        CurrentHealth -= damageAmount;
        Debug.Log($"UFO took {damageAmount} damage. Remaining Health: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    ///<summary>
    ///Triggers the respawn cinematic via RespawnManager.
    ///Game Over only occurs if no respawn is available (base destroyed).
    ///</summary>
    private void Die()
    {
        Debug.Log("UFO destroyed! Starting respawn sequence...");

        if (RespawnManager.Instance != null)
        {
            RespawnManager.Instance.TriggerRespawn(this);
        }
        else
        {
            Debug.LogError("UFO: RespawnManager not found in scene! Falling back to GameOver.");
            if (GameOverManager.Instance != null)
                GameOverManager.Instance.TriggerGameOver();
        }
    }

    // Methods called by RespawnManager to control the UFO during the cinematic  

    ///<summary>
    ///Activates or deactivates UFO controls and collisions during the respawn cinematic.
    ///</summary>
    public void SetActive(bool active)
    {
        _isActive = active;
        GetComponent<SpriteRenderer>().enabled = active;

        // Disable the collider so enemies don't hit an invisible UFO
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = active;

        // Stop physics movement
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    ///<summary>
    ///Restores UFO health to maximum. Called by RespawnManager on reappearance.
    ///</summary>
    public void RestoreHealth()
    {
        CurrentHealth = maxHealth;
        Debug.Log($"UFO health restored to {maxHealth}.");
    }

    ///<summary>
    ///Enables or disables invincibility frames. Called by RespawnManager after respawn.
    ///</summary>
    public void SetInvincible(bool invincible)
    {
        _isInvincible = invincible;
    }

    // Status effects

    ///<summary>
    ///Immobilizes the player for a given duration. Ignored if already stunned.
    ///</summary>
    public void ApplyStun(float duration)
    {
        if (!_isStunned) StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        _isStunned = true;
        Debug.Log($"UFO stunned for {duration}s!");
        yield return new WaitForSeconds(duration);
        _isStunned = false;
        Debug.Log("UFO stun ended.");
    }

    ///<summary>
    ///Applies a damage-over-time burn effect. Ignored if already burning.
    ///</summary>
    public void ApplyBurn(float dps, float duration)
    {
        if (!_isBurning) StartCoroutine(BurnRoutine(dps, duration));
    }

    private IEnumerator BurnRoutine(float dps, float duration)
    {
        _isBurning = true;
        Debug.Log($"UFO burning! {dps} dps for {duration}s.");
        float elapsed = 0f;
        while (elapsed < duration)
        {
            TakeDamage(dps * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _isBurning = false;
        Debug.Log("UFO burn ended.");
    }
}