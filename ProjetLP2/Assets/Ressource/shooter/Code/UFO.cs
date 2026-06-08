using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
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

    // Stun & Burn state flags
    private bool _isStunned = false;
    private bool _isBurning = false;

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
    ///</summary>
    void Update()
    {
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
            Debug.Log("UFO fired a laser!");
        }
        else
        {
            Debug.LogWarning("UFO: Laser Prefab or Fire Point reference missing in Inspector.");
        }
    }

    ///<summary>
    ///Inflicts a specific amount of damage to the player and checks for death condition.
    ///</summary>
    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Debug.Log($"UFO took {damageAmount} damage. Remaining Health: {CurrentHealth}");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    ///<summary>
    ///Immobilizes the player for a given duration. Ignored if already stunned.
    ///</summary>
    public void ApplyStun(float duration)
    {
        if (!_isStunned) StartCoroutine(StunRoutine(duration));
    }

    ///<summary>
    ///Coroutine that sets the stun flag for the specified duration then clears it.
    ///</summary>
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

    ///<summary>
    ///Coroutine that ticks burn damage every frame for the specified duration.
    ///</summary>
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

    ///<summary>
    ///Handles player destruction logic and prepares the respawn sequence.
    ///</summary>
    private void Die()
    {
        Debug.Log("UFO destroyed! Triggering respawn sequence...");
        // TODO: Disable player control and call the respawn cutscene manager here
        if (GameOverManager.Instance == null)
        {
            Debug.LogError("GameOverManager Instance is NULL! GameOverManager not found in scene.");
            return;
        }
        GameOverManager.Instance.TriggerGameOver();
    }
}