using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the player's paddle: manual left/right movement in normal play,
/// auto-follow of the ball in aesthetic mode, and the temporary speed/size
/// bonuses that can be applied to it.
/// </summary>
public class Paddle : MonoBehaviour
{
    public static Paddle Instance { get; private set; }

    private const float HorizontalBound = 9f;

    private float        translationSpeed;
    public  AudioClip     paddleSong;
    private AudioSource   audioSource;
    private bool          isAesthetic;
    private Vector3       originalScale;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        translationSpeed = 7f;
        audioSource      = GetComponent<AudioSource>();
        isAesthetic      = SpawnerBall.Instance.getIsEstetique();
        originalScale    = transform.localScale;
    }

    private void Update()
    {
        if (!isAesthetic)
        {
            if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < HorizontalBound)
            {
                transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
            }

            if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -HorizontalBound)
            {
                transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
            }
        }
        else
        {
            if (Ball.Instance != null)
            {
                float targetX = Ball.Instance.transform.position.x;
                float newX    = Mathf.MoveTowards(
                    transform.position.x,
                    targetX,
                    translationSpeed * Time.deltaTime
                );
                newX = Mathf.Clamp(newX, -HorizontalBound, HorizontalBound);
                transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BrickSpawner.setCoefficient(0);

        if (audioSource != null && paddleSong != null)
            audioSource.PlayOneShot(paddleSong);
    }

    /// <summary>Temporarily increases paddle movement speed by the given percentage.</summary>
    public void BonusSpeed(float percent, float duration)
    {
        StartCoroutine(SpeedCoroutine(percent, duration));
    }

    private IEnumerator SpeedCoroutine(float percent, float duration)
    {
        float bonus = translationSpeed * percent / 100f;
        translationSpeed += bonus;
        yield return new WaitForSeconds(duration);
        translationSpeed -= bonus;
    }

    /// <summary>Temporarily increases paddle width by the given percentage.</summary>
    public void BonusSize(float percent, float duration)
    {
        StartCoroutine(SizeCoroutine(percent, duration));
    }

    private IEnumerator SizeCoroutine(float percent, float duration)
    {
        transform.localScale = new Vector3(
            originalScale.x * (1f + percent / 100f),
            originalScale.y,
            originalScale.z);
        yield return new WaitForSeconds(duration);
        transform.localScale = originalScale;
    }
}