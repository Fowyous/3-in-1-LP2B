using UnityEngine;

/// <summary>
/// "Lucky" brick variant: changes sprite as it takes damage and, once
/// destroyed, grants the player a random bonus (or malus) picked from
/// a fixed set of effects.
/// </summary>
public class LuckyBrick : BrickParent
{
    [SerializeField] private Sprite[] damageSprites; 

    private SpriteRenderer spriteRenderer;
    
    protected override void Start()
    {
        base.Start();
        health         = 4;
        pointValue     = 50;
        coefficient    = 5;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Lucky bricks don't use the shared heal-then-damage flow from the
    /// base class: a healing ball heals them OR a damaging ball damages
    /// them, never both on the same hit (handled entirely in TakeDamage()).
    /// </summary>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        OneShot();

        if (!Ball.IsOneShot)
        {
            TakeDamage();
        }
    }

    /// <summary>
    /// Applies damage or healing depending on the ball type, updates the
    /// brick's sprite to reflect remaining health (clamped to the sprite
    /// array bounds), and triggers a random bonus/malus on destruction.
    /// </summary>
    protected override void TakeDamage()
    {
        if (!Ball.IsHealingBall)
        {
            health--;
        }
        else
        {
            health++;
        }

        if (health > 0)
        {
            int spriteIndex = Mathf.Min(health - 1, damageSprites.Length - 1);
            spriteRenderer.sprite = damageSprites[spriteIndex];
        }
        else
        {
            if (!isAesthetic)
            {
                Debug.Log("Points gagnés : " + pointValue);
                BrickSpawner.setCoefficient(coefficient);
                BrickSpawner.Instance.AddScore(pointValue);
                GameOverControllerBrickBreaker.editNumberLucky(1);
            }
            TriggerRandomEffect();
            Destroy(gameObject);
        }
    }

    /// <summary>Picks one of the 7 possible effects at random and applies it.</summary>
    private void TriggerRandomEffect()
    {
        LuckyEffect effect = (LuckyEffect)Random.Range(0, 7);

        switch (effect)
        {
            case LuckyEffect.BonusMaxHealth:
                SpawnerBall.healMaxLives(1);
                BonusManager.Instance?.Register("+1 Vie", 2f);
                break;

            case LuckyEffect.BonusPaddleSpeed:
                Paddle.Instance.BonusSpeed(30f, 3f);
                BonusManager.Instance?.Register("Vitesse Paddle", 3f);
                break;

            case LuckyEffect.BonusPaddleSize:
                Paddle.Instance.BonusSize(50f, 5f);
                BonusManager.Instance?.Register("Taille Paddle", 5f);
                break;

            case LuckyEffect.BonusOneShot:
                Ball.Instance.ActivateOneShot(4f);
                BonusManager.Instance?.Register("One Shot", 4f);
                break;

            case LuckyEffect.MalusBallSpeedIncrease:
                Ball.Instance.ActivateBallSpeedBoost(40f, 3f);
                BonusManager.Instance?.Register("Balle Rapide", 3f);
                break;

            case LuckyEffect.MalusHealingBall:
                Ball.Instance.ActivateHealingBall(5f);
                BonusManager.Instance?.Register("Balle Soignante", 5f);
                break;

            case LuckyEffect.Duplicate:
                bool   costLife = SpawnerBall.Instance.DuplicateBall();
                string label    = costLife ? "Duplication (-1 vie)" : "Duplication (gratuite)";
                BonusManager.Instance?.Register(label, 2f);
                break;
        }
    }
}