using UnityEngine;

public class Asteroid : MonoBehaviour
{
  private Rigidbody2D rb;

  [SerializeField] private float moveSpeed = 5f;        // Speed moving left
  [SerializeField] private float rotationSpeed = 8f;    // Speed of rotationSpeed

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    rb = GetComponent<Rigidbody2D>();

    // Set initial movement velocity (negative X = moving left)
    rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);

    // Set rotation (positive value = counterclockwise)
    rb.angularVelocity = rotationSpeed;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
