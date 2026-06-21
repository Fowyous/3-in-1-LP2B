using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Controls the player-controlled basket in AppleCatcher: left/right
/// movement (with inverted-controls support), score/coefficient tracking,
/// and the temporary speed/inversion effects applied by special apples.
/// </summary>
public class CatchboyController : MonoBehaviour
{
    public static CatchboyController Instance { get; private set; }

    [FormerlySerializedAs("ref_animator")]
    public Animator animator;
    public TextMeshPro scoreText;
    public AudioClip   collectedSound;
    private AudioSource audioSource;

    private static readonly int Idle      = Animator.StringToHash("Idle");
    private static readonly int Backwards = Animator.StringToHash("Backwards");
    private static readonly int Forwards  = Animator.StringToHash("Forwards");

    private static int   score;
    private static int   coefficient;
    private static float baseSpeed = 7f;
    private static float horizontalBound = 7.2f;

    private float translationSpeed;
    private bool  isInverted = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score             = 0;
        coefficient       = 0;
        translationSpeed  = baseSpeed;
        scoreText.SetText("score : " + score);
        audioSource       = GetComponent<AudioSource>();
    }

    private void Update()
    {
        int direction = isInverted ? -1 : 1;

        if (Keyboard.current.rightArrowKey.isPressed && CanMove(direction))
        {
            Move(direction);
        }
        else if (Keyboard.current.leftArrowKey.isPressed && CanMove(-direction))
        {
            Move(-direction);
        }
        else
        {
            animator.SetTrigger(Idle);
        }
    }

    /// <summary>Checks whether the basket can still move further in the given direction without crossing the play field bounds.</summary>
    private bool CanMove(int direction)
    {
        return direction > 0 ? transform.position.x < horizontalBound : transform.position.x > -horizontalBound;
    }

    /// <summary>Moves the basket one step in the given direction and plays the matching animation.</summary>
    private void Move(int direction)
    {
        transform.Translate(Vector3.right * (direction * Time.deltaTime * translationSpeed));
        animator.SetTrigger(direction > 0 ? Forwards : Backwards);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (audioSource != null && collectedSound != null)
            audioSource.PlayOneShot(collectedSound);
    }

    /// <summary>Adds to the running combo coefficient (or resets it to 0 if coef is 0), clamped to a minimum of 1 otherwise.</summary>
    public void editCoefficient(int coef)
    {
        if (coef == 0)
        {
            coefficient = 0;
        }
        else
        {
            coefficient += coef;
            if (coefficient <= 0)
            {
                coefficient = 1;
            }
        }
    }

    /// <summary>Adds points to the score, multiplied by the current coefficient, and refreshes the score display.</summary>
    public void AddScore(int value)
    {
        score += value * coefficient;

        if (coefficient <= 1)
        {
            scoreText.SetText("score : " + score);
        }
        else
        {
            scoreText.SetText("score : " + score + "*" + coefficient);
        }
    }

    /// <summary>Temporarily multiplies movement speed by the given factor.</summary>
    public void editSpeed(float value, float duration)
    {
        string label = value > 1f ? "⚡ Vitesse x" + value : "🐢 Vitesse x" + value;
        BonusManager.Instance.Register(label, duration);
        StartCoroutine(SpeedEffect(baseSpeed * value, duration));
    }

    /// <summary>Temporarily inverts left/right controls.</summary>
    public void ApplyInvertedControls(float duration)
    {
        BonusManager.Instance.Register("↔ Contrôles inversés", duration);
        StartCoroutine(InvertEffect(duration));
    }

    private IEnumerator SpeedEffect(float newSpeed, float duration)
    {
        translationSpeed = newSpeed;
        yield return new WaitForSeconds(duration);
        translationSpeed = baseSpeed;
    }

    private IEnumerator InvertEffect(float duration)
    {
        isInverted = true;
        yield return new WaitForSeconds(duration);
        isInverted = false;
    }

    public static int scoreGet()
    {
        return score;
    }
}