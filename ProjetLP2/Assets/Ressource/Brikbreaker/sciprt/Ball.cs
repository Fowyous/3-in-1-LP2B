// Ball.cs
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const float DeathThreshold = -8f;
    private bool isDead = false;

    void Update()
    {
        if (!isDead && transform.position.y < DeathThreshold)
        {
            isDead = true;
            Destroy(gameObject);
        }
    }
}