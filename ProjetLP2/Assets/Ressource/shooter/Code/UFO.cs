using UnityEngine;
using UnityEngine.InputSystem;
using static ShooterConstants;

public class UFO : MonoBehaviour
{
  [SerializeField] private float SPEED = 5f;

  /// <summary>
  /// Handles player movement with keyboard input and boundary constraints.
  /// </summary>
  void HandleMovement()
  {
    Vector3 moveDirection = Vector3.zero;

    // Get input from arrow keys
    if (Keyboard.current.rightArrowKey.isPressed) moveDirection.x = 1;
    if (Keyboard.current.leftArrowKey.isPressed) moveDirection.x = -1;
    if (Keyboard.current.upArrowKey.isPressed) moveDirection.y = 1;
    if (Keyboard.current.downArrowKey.isPressed) moveDirection.y = -1;

    // Apply movement with frame-rate independence
    transform.Translate(moveDirection * Time.deltaTime * SPEED);

    // Clamp to play area to prevent going off-screen
    Vector3 pos = transform.position;
    pos.x = Mathf.Clamp(pos.x, -ShooterConstants.GameLimit.x, ShooterConstants.GameLimit.x);
    pos.y = Mathf.Clamp(pos.y, -ShooterConstants.GameLimit.y, ShooterConstants.GameLimit.y);
    transform.position = pos;
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    HandleMovement();

    Debug.Log("bb");



  }
}
