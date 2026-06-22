using UnityEngine;
using UnityEngine.InputSystem;
using System;
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


  [Header("Animation")]
  [SerializeField] private Animator animator;
  private int stunHash;

  [Header("Audio")]
  [SerializeField] private AudioClip shootSound;
  [SerializeField] private AudioClip deathCry;

  private AudioSource audioSource;

  public float MaxHealth => maxHealth;
  public float CurrentHealth { get; private set; }

  ///<summary>
  ///Fired whenever health changes. Passes (currentHealth, maxHealth).
  ///Subscribed to by UFOHealthUI to update the life icons display.
  ///</summary>
  public event Action<float, float> OnHealthChanged;

  private float nextFireTime = 0f;
  private Rigidbody2D rb;

  private bool _isStunned = false;
  private bool _isBurning = false;
  private bool _isInvincible = false;
  private bool _isActive = true;

  void Start()
  {
    CurrentHealth = maxHealth;
    rb = GetComponent<Rigidbody2D>();
    if (rb == null)
      Debug.LogError("UFO: Rigidbody2D component missing on this GameObject!");

    // Notify UI of the initial health value
    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

    // For the Energyball damage
    stunHash = Animator.StringToHash("IsStunned");

    // Taking the audio component
    audioSource = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (!_isActive) return;
    HandleMovement();
    HandleShooting();
  }

  ///<summary>
  ///Handles player movement. Vertical clamp uses separate top/bottom limits:
  ///yTop is reduced so the UFO never overlaps the HUD Canvas at the top of the screen,
  ///while yBottom stays at the full screen edge (no UI element down there).
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
    pos.y = Mathf.Clamp(pos.y, -ShooterConstants.GameLimit.yBottom, ShooterConstants.GameLimit.yTop);
    transform.position = pos;
  }

  private void HandleShooting()
  {
    if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime)
    {
      PlaySound(shootSound);
      ShootLaser();
      nextFireTime = Time.time + fireRate;
    }
  }

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

  public void TakeDamage(float damageAmount)
  {
    if (_isInvincible || !_isActive) return;

    CurrentHealth -= damageAmount;
    CurrentHealth = Mathf.Max(CurrentHealth, 0f);
    Debug.Log($"UFO took {damageAmount} damage. Remaining Health: {CurrentHealth}");

    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

    if (CurrentHealth <= 0)
    {
      Die();
    }
  }

  private void Die()
  {
    Debug.Log("UFO destroyed! Starting respawn sequence...");

    PlaySound(deathCry);

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

  public void SetActive(bool active)
  {
    _isActive = active;
    GetComponent<SpriteRenderer>().enabled = active;

    Collider2D col = GetComponent<Collider2D>();
    if (col != null) col.enabled = active;

    if (rb != null) rb.linearVelocity = Vector2.zero;
  }

  public void RestoreHealth()
  {
    CurrentHealth = maxHealth;
    Debug.Log($"UFO health restored to {maxHealth}.");
    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
  }

  public void SetInvincible(bool invincible)
  {
    _isInvincible = invincible;
  }

  public void ApplyStun(float duration)
  {
    if (!_isStunned) StartCoroutine(StunRoutine(duration));
  }

  private IEnumerator StunRoutine(float duration)
  {
    Debug.Log("stunned");
    Debug.Log("stun hash : " + stunHash);
    animator.SetBool(stunHash, true);  // START stun animation
    _isStunned = true;
    yield return new WaitForSeconds(duration);
    animator.SetBool(stunHash, false); // STOP stun animation
    _isStunned = false;
  }

  public void ApplyBurn(float dps, float duration)
  {
    if (!_isBurning) StartCoroutine(BurnRoutine(dps, duration));
  }

  private IEnumerator BurnRoutine(float dps, float duration)
  {
    _isBurning = true;
    float elapsed = 0f;
    while (elapsed < duration)
    {
      TakeDamage(dps * Time.deltaTime);
      elapsed += Time.deltaTime;
      yield return null;
    }
    _isBurning = false;
  }

  private void PlaySound(AudioClip clip)
  {
    audioSource.PlayOneShot(clip);
  }
}