using UnityEngine;

public class SimpleBrick : MonoBehaviour
{
    private int healthSimple = 1;
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
    
    protected void takeDamage()
    {
        healthSimple--;
        
        if (healthSimple <= 0)
        {
            Debug.Log("Points gagnés : " + pointValue);
            BrickSpawner.setCoefficient(coefficient);
            BrickSpawner.Instance.AddScore(pointValue);
            controllerTexte.editNumberSimple(1);
            Destroy(gameObject);
        }
    }
    
    protected void healBlock()
    {
        if (Ball.IsHealingBall)
        {
            healthSimple++;
        }
    }

    protected void OneShoot()
    {
        if (Ball.IsOneShot)
        {
            BrickSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}