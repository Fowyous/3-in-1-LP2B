using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PanierScript : MonoBehaviour
{
    private float translationSpeed;
    private float verticalSpeed;
    private static int score;
    public TextMeshPro scoreText;
    public AudioClip collectedSound;
    private AudioSource audioSource;

    void Start()
    {
        score = 0;
        translationSpeed = 7f;
        verticalSpeed = 1f;
        scoreText.SetText("score : " + score);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < 7.2)
        {
            transform.Translate(Vector3.right * Time.deltaTime * translationSpeed);
        }

        if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -7.2)
        {
            transform.Translate(Vector3.left * Time.deltaTime * translationSpeed);

        }

        if (Keyboard.current.upArrowKey.isPressed && transform.position.y < 3.8)
        {
            transform.Translate(Vector3.up * Time.deltaTime * verticalSpeed);
        }

        if (Keyboard.current.downArrowKey.isPressed && transform.position.y > -3.8)
        {
            transform.Translate(Vector3.down * Time.deltaTime * verticalSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        score++;
        scoreText.SetText("score : " + score);
        audioSource.PlayOneShot(collectedSound);
    }

    public static int scoreGet()
    {
        return score; 
    }
}
