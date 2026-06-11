using UnityEngine;

public class luckyBlock : Block
{
    private int healthLucky = 4;
    private static int pointValue   = 50;
    private static int coefficient  = 5;
    
    private const int BonusMaxhealt  = 0;
    private const int BonusSpeed = 1;
    private const int BonusSize   = 2;
    private const int BonusOneShot  = 3;
    private const int MalusBallSpeedIncrease = 4;
    private const int MalusHealingBlock = 5;
    private const int Duplicate = 6;

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
        healthLucky--;
        
        if (healthLucky <= 0)
        {
            EditBonusMalus();
            BlockSpawner.setCoefficient(coefficient);
            BlockSpawner.Instance.AddScore(pointValue);
            Destroy(gameObject);
        }
    }

    private static void EditBonusMalus()
    {
        int roll = Random.Range(0, 7);

        switch (roll)
        {
            case BonusMaxhealt: 
                SpawnerBall.healMaxLives(1);
                break;
            case BonusSpeed: 
                Paddle.Instance.BonusSpeed(30f, 3f);
                break;
            case BonusSize:
                Paddle.Instance.BonusSize(50f, 5f);
                break;
            case BonusOneShot: 
                Ball.Instance.ActivateOneShot(4f);
                break;
            case MalusBallSpeedIncrease:
                Ball.Instance.ActivateBallSpeedBoost(40f, 3f);
                break;
            case MalusHealingBlock:
                Ball.Instance.ActivateHealingBall(5f);
                break;
            case Duplicate:
                SpawnerBall.Instance.DuplicateBall();
                break;
            default:
                break;
        }
    }
}