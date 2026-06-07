using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Catchboy : MonoBehaviour
{
    public Animator ref_animator;
    private float translationSpeed;
    private static int score;
    private static int coefficient;
    public TextMeshPro scoreText;
    public AudioClip collectedSound;
    private AudioSource audioSource;

    private bool isInverted = false;
    private static float baseSpeed = 7f;
    private static float bordure = 7.2f;

    public static Catchboy Instance;
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Backwards = Animator.StringToHash("Backwards");
    private static readonly int Forwards = Animator.StringToHash("Forwards");

    void Awake()
    {
        Instance = this;
    }

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 0;
        coefficient = 0;
        translationSpeed = baseSpeed;
        scoreText.SetText("score : " + score);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    { if (isInverted)
        {
            if (Keyboard.current.rightArrowKey.isPressed && transform.position.x > -bordure) 
            {
                transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
                ref_animator.SetTrigger(Backwards);
            } 
            else if (Keyboard.current.leftArrowKey.isPressed && transform.position.x < bordure)
            {
                transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
                ref_animator.SetTrigger(Forwards);
            }
            else
            {
                ref_animator.SetTrigger(Idle);
            }
        }
        else
        {
            if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < bordure) 
            {
                transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
                ref_animator.SetTrigger(Forwards);
            }
            else if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -bordure)
            {
                transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
                ref_animator.SetTrigger(Backwards);
            }
            else
            {
                ref_animator.SetTrigger(Idle);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        audioSource.PlayOneShot(collectedSound);
    }

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

    public void editSpeed(float  value, float duration)
    {
        string label = value > 1f ? "⚡ Vitesse x" + value : "🐢 Vitesse x" + value;
        BonusPrint.Instance.ShowEffect(label, duration);
        StartCoroutine(SpeedEffect(baseSpeed * value, duration));
    }
    
    public void ApplyInvertedControls(float duration)
    {
        BonusPrint.Instance.ShowEffect("↔ Contrôles inversés", duration);
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
