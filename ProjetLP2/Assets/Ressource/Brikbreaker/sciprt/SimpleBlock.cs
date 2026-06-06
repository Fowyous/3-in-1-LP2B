using UnityEngine;

public class SimpleBlock : Block
{
    private int health = 1;
    private static int pointValue = 10;
    private static int coefficient = 1;

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        healBlock();
        OneShoot();
        if (!Ball.IsOneShot)
        {
            takeDamage();
        }
    }
    
    protected override void takeDamage()
    {
        health--;
        
        if (health <= 0)
        {
            Debug.Log("Points gagnés : " + pointValue);
            BlockSpawner.setCoefficient(coefficient);
            BlockSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}