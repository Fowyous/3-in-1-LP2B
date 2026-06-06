using UnityEngine;
using UnityEngine.InputSystem;
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
    ///</summary>
    private void HandleMovement()
    {
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
    ///Handles player destruction logic and prepares the respawn sequence.
    ///</summary>
    private void Die()
    {
        Debug.Log("UFO destroyed! Triggering respawn sequence...");
        // TODO: Disable player control and call the respawn cutscene manager here
    }
}