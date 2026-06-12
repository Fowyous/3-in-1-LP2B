using UnityEngine;

public class HardBlock : MonoBehaviour
{
    private int healthHard = 2;
    private static int pointValue = 25;
    private static int coefficient = 2;
    private Color baseColor;

    private void Start()
    {
        baseColor = GetComponent<SpriteRenderer>().color;
    }
    
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        healBlock();
        OneShoot();
        if (!Ball.IsOneShot)
        {
            takeDamage();
        }
    }

    protected void takeDamage()
    {
        healthHard--;

        if (healthHard > 0)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            GetComponent<SpriteRenderer>().color = new Color(c.r * 0.5f, c.g * 0.5f, c.b * 0.5f);
        }
        else
        {
            Debug.Log("Points gagnés : " + pointValue);
            BlockSpawner.setCoefficient(coefficient);
            BlockSpawner.Instance.AddScore(pointValue);
            controllerTexte.editNumberHard(1);
            Destroy(gameObject);
        }
    }
    
    protected void healBlock()
    {
        if (Ball.IsHealingBall)
        {
            healthHard++;
        }
    }

    protected void OneShoot()
    {
        if (Ball.IsOneShot)
        {
            BlockSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}