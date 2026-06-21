using UnityEngine;

public class GigaHardBrick : MonoBehaviour
{
    private int healthGIGAHard = 6;
    private static int pointValue = 100;
    private static int coefficient = 10;
    [SerializeField] private Sprite[] Sprite; 
    private SpriteRenderer blockSprite;

    private void Start()
    {
        blockSprite = GetComponent<SpriteRenderer>();
    }
    
    protected void OnCollisionEnter2D()
    {
        OneShoot();
        if (!Ball.IsOneShot)
        {
            takeDamage();
        }
    }

    protected void takeDamage()
    {
        if (!Ball.IsHealingBall)
        {
            healthGIGAHard -= 1;
        }
        else
        {
            healthGIGAHard += 1;
        }

        if (healthGIGAHard > 0)
        {
            blockSprite.sprite = Sprite[healthGIGAHard-1];
        }
        else
        {
            Debug.Log("Points gagnés : " + pointValue);
            BrickSpawner.setCoefficient(coefficient);
            BrickSpawner.Instance.AddScore(pointValue);
            controllerTexte.editNumberHard(1);
            Destroy(gameObject);
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