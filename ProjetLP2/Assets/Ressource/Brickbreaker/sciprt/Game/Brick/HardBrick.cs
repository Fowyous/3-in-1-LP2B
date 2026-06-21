using UnityEngine;

public class HardBrick : MonoBehaviour
{
    private int healthHard = 2;
    private static int pointValue = 25;
    private static int coefficient = 2;
    [SerializeField] private Sprite NormalSprite; 
    [SerializeField] private Sprite damageSprite;
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
            healthHard -= 1;
        }
        else
        {
            healthHard += 1;
        }

        if (healthHard > 0)
        {
            if (!Ball.IsHealingBall)
            {
                blockSprite.sprite = damageSprite;
            }
            else
            {
                blockSprite.sprite = NormalSprite;
            }
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