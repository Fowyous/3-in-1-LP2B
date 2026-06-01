using UnityEngine;

public class HardBlock : MonoBehaviour
{
    private int   health     = 2;
    private int   pointValue = 25;
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
            Destroy(gameObject);
        }
    }
}