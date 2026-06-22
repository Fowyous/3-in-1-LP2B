using UnityEngine;
using System;

public class BaseManager : MonoBehaviour
{
  [Header("Base Stats")]
  [SerializeField] private float maxHealth = 300f;

  [Header("Regeneration Settings")]
  [Tooltip("HP regained per second. Ex: 2 = full regen from 0 to 300 takes 150 seconds.")]
  [SerializeField] private float regenAmountPerSecond = 2f;
  [Tooltip("Seconds without taking damage before regeneration starts.")]
  [SerializeField] private float regenDelay = 5f;

  public float MaxHealth => maxHealth;
  public float CurrentHealth { get; private set; }

  ///<summary>
  ///Fired whenever the base health changes (damage or regen). Passes (currentHealth, maxHealth).
  ///Subscribed to by BaseHealthBar to update the UI fill amount.
  ///</summary>
  public event Action<float, float> OnHealthChanged;

  private float timeOfLastDamage;

  void Start()
  {
    CurrentHealth = maxHealth;
    timeOfLastDamage = -regenDelay;

    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
  }

  void Update()
  {
    if (Time.time - timeOfLastDamage >= regenDelay && CurrentHealth < maxHealth)
    {
      ApplyPassiveRegeneration();
    }
  }

  private void ApplyPassiveRegeneration()
  {
    CurrentHealth += regenAmountPerSecond * Time.deltaTime;
    CurrentHealth = Mathf.Min(CurrentHealth, maxHealth);

    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
  }

  public void TakeDamage(float damageAmount)
  {
    CurrentHealth -= damageAmount;
    CurrentHealth = Mathf.Max(CurrentHealth, 0f); // Clamp immediately to avoid negative display
    timeOfLastDamage = Time.time;

    Debug.Log($"Base attacked! Took {damageAmount} damage. Health remaining: {CurrentHealth:F1}/{maxHealth}");

    OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

    if (CurrentHealth <= 0)
    {
      TriggerGameOver();
    }
  }

  private void TriggerGameOver()
  {
    Debug.LogWarning("Base Destroyed!");

    if (RespawnManager.Instance != null)
    {
      RespawnManager.Instance.CanRespawn = false;
      Debug.Log("BaseManager: Respawn disabled. Player must survive to avoid Game Over.");
    }
    else
    {
      Debug.LogWarning("BaseManager: RespawnManager not found, triggering Game Over directly.");
      if (GameOverManager.Instance != null)
        GameOverManager.Instance.TriggerGameOver();
    }
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    IEnemy enemy = collision.GetComponent<IEnemy>();
    if (enemy != null)
    {
      float totalDamage = enemy.Health + enemy.Damage;
      TakeDamage(totalDamage);
      Destroy(collision.gameObject);
    }
  }
}
