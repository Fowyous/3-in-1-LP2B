using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PanierScript : MonoBehaviour
{
    private float translationSpeed;
    private static int score;
    public TextMeshPro scoreText;
    public AudioClip collectedSound;
    private AudioSource audioSource;

    private bool isInverted = false;
    private static float baseSpeed = 7f;
    private static float bordure = 7.2f;

    public static PanierScript Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        translationSpeed = baseSpeed;
        scoreText.SetText("score : " + score);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isInverted)
        {
            if (Keyboard.current.rightArrowKey.isPressed && transform.position.x > -bordure) 
            {
                transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
            }

            if (Keyboard.current.leftArrowKey.isPressed && transform.position.x < bordure)
            {
                transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
            }
        }
        else
        {
            if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < bordure) 
            {
                transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
            }

            if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -bordure)
            {
                transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        audioSource.PlayOneShot(collectedSound);
    }
    
    public void AddScore(int value)
    {
        score += value;
        scoreText.SetText("score : " + score);
    }

    public void editSpeed(float  value, float duration)
    {
        StartCoroutine(SpeedEffect(baseSpeed * value, duration));
    }
    
    public void ApplyInvertedControls(float duration)
    {
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