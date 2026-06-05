// Ball.cs
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float DeathThreshold = -8f;
    private bool isDead = false;
    private float speed;
    private Vector3 velocity;

    protected virtual void Start()
    {
        speed = 8f;
        velocity = new Vector3(0f, -1f, 0f);
    }
    
    void Update()
    {
        if (!isDead && transform.position.y < DeathThreshold)
        {
            isDead = true;
            Destroy(gameObject);
        }
        transform.Translate(velocity * (Time.deltaTime * speed));
    }

    void OnCollisionEnter(Collision collision)
    {
        velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
    }
}