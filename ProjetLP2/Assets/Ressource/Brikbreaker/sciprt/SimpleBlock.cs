using UnityEngine;

public class SimpleBlock : MonoBehaviour
{
    private int health     = 1;
    private int pointValue = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Debug.Log("Points gagnés : " + pointValue);
            Destroy(gameObject);
        }
    }
}