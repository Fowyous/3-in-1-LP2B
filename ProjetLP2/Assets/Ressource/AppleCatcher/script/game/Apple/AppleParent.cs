using UnityEngine;

/// <summary>
/// Base class for all falling apple types in AppleCatcher.
/// Handles vertical movement, catch/miss detection, and exposes
/// virtual hooks for subclasses to customize speed, scoring and
/// "missed" behavior without duplicating the main game loop.
/// </summary>
public class AppleParent : MonoBehaviour
{
    [Header("Settings")]
    public bool isAesthetic;
    public int coefficient = 1;

    [SerializeField] protected int   score     = 1;
    [SerializeField] protected int   damage    = -1;
    [SerializeField] protected float fallLimit = 6f;

    protected float attackThreshold;
    private static float speed;

    protected virtual float Speed
    {
        get => speed;
        set => speed = value;
    }

    protected virtual void Start()
    {
        Speed           = 8f;
        attackThreshold = 0f;
        coefficient     = 1;
        fallLimit       = 6f;
        score           = 1;
        damage          = -1;
    }

    private void Update()
    {
        UpdateSpeed();

        transform.Translate(Vector3.down * (Speed * Time.deltaTime));

        if (transform.position.y < -fallLimit)
        {
            if (!isAesthetic)
            {
                OnMissed();
            }
            Destroy(gameObject);
        }
    }

    /// <summary>Override to make speed evolve over time or position (e.g. accelerate near the bottom).</summary>
    protected virtual void UpdateSpeed() { }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("catchboy"))
        {
            OnCaught();
            Destroy(gameObject);
        }
    }

    /// <summary>Called when the player catches this apple.</summary>
    protected virtual void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberApple(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        CatchboyController.Instance.AddScore(score);
    }

    /// <summary>Called when the apple falls past the fall limit without being caught.</summary>
    protected virtual void OnMissed()
    {
        CatchboyController.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }

    public void SetAesthetic(bool value)
    {
        isAesthetic = value;
    }

    public float GetFallLimit()
    {
        return fallLimit;
    }
}