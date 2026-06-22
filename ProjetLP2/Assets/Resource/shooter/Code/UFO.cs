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

  [Header("Power-Up Visuals")]
  [Tooltip("Child GameObject holding the shield sprite/animation. Activated while the Shield power-up is active.")]
  [SerializeField] private GameObject shieldVisual;

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

  // Power-up state
  private bool _hasShield = false;
  private float _baseFireRate;
  private float _baseSpeed;

  [Header("Special Attack (Bomb)")]
  [Tooltip("Number of Bomb fragments required to trigger the special attack.")]
  [SerializeField] private int bombsRequired = 4;
  public int BombCount { get; private set; } = 0;

  ///<summary>
  ///Fired whenever the bomb count changes. Passes (currentCount, required).
  ///Subscribed to by BombHUD to update the icon display.
  ///</summary>
  public event System.Action<int, int> OnBombCountChanged;

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

    // Remember base stats so power-ups can be reverted correctly afterwards
    _baseFireRate = fireRate;
    _baseSpeed = speed;
  }

  void Update()
  {
    if (!_isActive) return;
    HandleMovement();
    HandleShooting();
    HandleSpecialAttackInput();
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

  ///<summary>
  ///Listens for the 'T' key to trigger the special attack.
  ///Does nothing if the player hasn't collected enough Bomb fragments yet.
  ///</summary>
  private void HandleSpecialAttackInput()
  {
    if (Keyboard.current.tKey.wasPressedThisFrame)
    {
      TriggerSpecialAttack();
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

  // -------------------------------------------------------------------------
  // Power-ups
  // -------------------------------------------------------------------------

  ///<summary>
  ///Dispatches a collected power-up to its corresponding effect.
  ///Called by PowerUpPickup when the UFO touches a pickup.
  ///</summary>
  public void ApplyPowerUp(PowerUpType type)
  {
    switch (type)
    {
      case PowerUpType.RapidFire:
        StartCoroutine(RapidFireRoutine(8f));
        break;
      case PowerUpType.SpeedBoost:
        StartCoroutine(SpeedBoostRoutine(8f));
        break;
      case PowerUpType.Shield:
        StartCoroutine(ShieldRoutine(6f));
        break;
      case PowerUpType.Bomb:
        CollectBombFragment();
        break;
      case PowerUpType.Heal:
        ApplyHeal(0.5f); // Heals 50% of max health instantly
        break;
    }
  }

  ///<summary>
  ///Doubles fire rate (halves the cooldown) for the given duration.
  ///</summary>
  private IEnumerator RapidFireRoutine(float duration)
  {
    fireRate = _baseFireRate / 2f;
    yield return new WaitForSeconds(duration);
    fireRate = _baseFireRate;
  }

  ///<summary>
  ///Increases movement speed by 50% for the given duration.
  ///</summary>
  private IEnumerator SpeedBoostRoutine(float duration)
  {
    speed = _baseSpeed * 1.5f;
    yield return new WaitForSeconds(duration);
    speed = _baseSpeed;
  }

  ///<summary>
  ///Grants temporary full invincibility (stacks safely with respawn invincibility).
  ///Activates the shield visual/animation for the duration, if assigned.
  ///</summary>
  private IEnumerator ShieldRoutine(float duration)
  {
    _hasShield = true;
    SetInvincible(true);
    if (shieldVisual != null) shieldVisual.SetActive(true);

    yield return new WaitForSeconds(duration);

    _hasShield = false;
    if (shieldVisual != null) shieldVisual.SetActive(false);
    if (!_hasShield) SetInvincible(false);
  }

  ///<summary>
  ///Instantly restores a percentage of max health.
  ///</summary>
  private void ApplyHeal(float percentage)
  {
    float healAmount = maxHealth * percentage;
    CurrentHealth = Mathf.Min(CurrentHealth + healAmount, maxHealth);
    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    Debug.Log($"UFO healed for {healAmount:F1}. Current Health: {CurrentHealth}/{maxHealth}");
  }

  ///<summary>
  ///Adds one Bomb fragment to the player's stock, capped at bombsRequired.
  ///Notifies the HUD so it can update the icon display.
  ///</summary>
  private void CollectBombFragment()
  {
    BombCount = Mathf.Min(BombCount + 1, bombsRequired);
    OnBombCountChanged?.Invoke(BombCount, bombsRequired);
    Debug.Log($"UFO collected a Bomb fragment. {BombCount}/{bombsRequired}");
  }

  ///<summary>
  ///Triggers the special attack (wipes out every enemy on screen) if the
  ///player has collected enough Bomb fragments. Does nothing otherwise.
  ///Resets the bomb count back to 0 afterwards and updates the HUD.
  ///</summary>
  public void TriggerSpecialAttack()
  {
    if (BombCount < bombsRequired)
    {
      Debug.Log($"UFO: Special attack not available yet ({BombCount}/{bombsRequired} bombs).");
      return;
    }

    if (SpecialAttackManager.Instance != null)
    {
      SpecialAttackManager.Instance.DetonateAllEnemies();
    }
    else
    {
      Debug.LogError("UFO: SpecialAttackManager not found in scene!");
    }

    BombCount = 0;
    OnBombCountChanged?.Invoke(BombCount, bombsRequired);
  }
}