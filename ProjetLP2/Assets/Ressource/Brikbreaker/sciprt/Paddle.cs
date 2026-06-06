using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    public static Paddle Instance { get; private set; }

    private float translationSpeed;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        translationSpeed = 7f;
    }

    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < 9.5f)
        {
            transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
        }

        if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -9.5f)
        {
            transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        BlockSpawner.setCoefficient(0);
    }
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

    public void BonusSize(float percent, float duration)
    {
        StartCoroutine(SizeCoroutine(percent, duration));
    }

    private IEnumerator SizeCoroutine(float percent, float duration)
    {
        Vector3 original = transform.localScale;
        transform.localScale = new Vector3(
            original.x * (1f + percent / 100f),
            original.y,
            original.z);
        yield return new WaitForSeconds(duration);
        transform.localScale = original;
    }
}