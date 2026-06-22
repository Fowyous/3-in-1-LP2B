using UnityEngine;

/// <summary>
/// Base class for all "simple" bricks in BrickBreaker. Handles ball
/// collision, healing/damage from special ball types, and destruction
/// once health reaches zero. Subclasses can override TakeDamage() (and
/// OnCollisionEnter2D() if needed) to add custom visuals or logic, or
/// just override OnBrickDestroyed() to hook into the destruction moment
/// without rewriting the whole damage flow.
/// </summary>
public class BrickParent : MonoBehaviour
{
    protected bool isAesthetic;
    
    [Header("Stats")]
    [SerializeField] protected int health = 1;
    [SerializeField] protected int pointValue = 1;
    [SerializeField] protected int coefficient = 1;

    [Header("Audio")]
    public AudioClip collisionBlockSong;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isAesthetic = SpawnerBall.Instance.getIsEstetique();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        HealBlock();
        OneShot();

        if (!Ball.IsOneShot)
        {
            TakeDamage();
        }
    }

    /// <summary>Restores 1 health point if the ball that hit this brick is a healing ball.</summary>
    protected void HealBlock()
    {
        if (Ball.IsHealingBall)
        {
            health++;
        }
    }

    /// <summary>Instantly destroys this brick and awards its full point value if the ball is a one-shot ball.</summary>
    protected void OneShot()
    {
        if (Ball.IsOneShot)
        {
            BrickSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }

    /// <summary>Reduces health by 1 and destroys the brick once it reaches zero, awarding points and playing the collision sound.</summary>
    protected virtual void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            if (audioSource != null && collisionBlockSong != null)
                audioSource.PlayOneShot(collisionBlockSong);
            if (!isAesthetic)
            {
                BrickSpawner.setCoefficient(coefficient);
                BrickSpawner.Instance.AddScore(pointValue);
            }
            OnBrickDestroyed();
            Destroy(gameObject);
        }
    }

    /// <summary>Override to run extra logic (stats tracking, debug logs, effects...) right before this brick is destroyed by damage.</summary>
    protected virtual void OnBrickDestroyed() { }
}