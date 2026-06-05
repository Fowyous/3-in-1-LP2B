using UnityEngine;

public class HardBlock : MonoBehaviour
{
    private static int health     = 2;
    private static int pointValue = 25;
    private Color baseColor;

    private void Start()
    {
        baseColor = GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TakeDamage();
    }

    private void TakeDamage()
    {
        health--;

        if (health > 0)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(c.r * 0.5f, c.g * 0.5f, c.b * 0.5f);
        }
        else
        {
            Debug.Log("Points gagnés : " + pointValue);
            BlockSpawner.setCoefficient(2);
            BlockSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}