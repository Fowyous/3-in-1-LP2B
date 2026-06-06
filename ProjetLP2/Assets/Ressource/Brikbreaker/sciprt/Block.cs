using UnityEngine;

public class Block : MonoBehaviour
{
    private int health;
    private static int pointValue;
    private static int coefficient;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void OnCollisionEnter(Collision collision)
    {
        healBlock();
        OneShoot();
        if (!Ball.IsOneShot)
        {
            takeDamage();
        }
    }

    protected void healBlock()
    {
        if (Ball.IsHealingBall)
        {
            health++;
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
    protected virtual void takeDamage()
    {
        health--;
        if (health <= 0)
        {
            BlockSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}
