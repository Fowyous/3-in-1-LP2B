using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MonoBehaviour
{
    private float translationSpeed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        translationSpeed = 7f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.isPressed && transform.position.x < 9.5)
        {
            transform.Translate(Vector3.right * (Time.deltaTime * translationSpeed));
        }
        if (Keyboard.current.leftArrowKey.isPressed && transform.position.x > -9.5)
        {
            transform.Translate(Vector3.left * (Time.deltaTime * translationSpeed));

        }
    }
}
