using UnityEngine;

public class BrickParent : MonoBehaviour
{
    private int health;
    private static int pointValue;
    private static int coefficient;
    public  AudioClip CollisionBlockSong;
    private static AudioSource audioSource; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    protected void OnCollisionEnter()
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
            BrickSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
    protected virtual void takeDamage()
    {
        health--;
        if (health <= 0)
        {
            if (audioSource != null && CollisionBlockSong != null)
                audioSource.PlayOneShot(CollisionBlockSong);
            BrickSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }
}