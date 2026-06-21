using UnityEngine;

/// <summary>
/// "Hard" brick variant: takes 2 hits to destroy, switching between a
/// normal and a damaged sprite depending on whether the last hit came
/// from a healing or a damaging ball.
/// </summary>
public class HardBrick : BrickParent
{
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite damageSprite;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        health         = 2;
        pointValue     = 25;
        coefficient    = 2;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Hard bricks don't use the shared heal-then-damage flow from the
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
    /// Applies damage or healing depending on the ball type, swaps the
    /// sprite to reflect the last hit type, and destroys the brick with
    /// score/coefficient once health reaches zero.
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
            spriteRenderer.sprite = !Ball.IsHealingBall ? damageSprite : normalSprite;
        }
        else
        {
            Debug.Log("Points gagnés : " + pointValue);
            BrickSpawner.setCoefficient(coefficient);
            BrickSpawner.Instance.AddScore(pointValue);
            GameOverControllerBrickBreaker.editNumberHard(1);
            Destroy(gameObject);
        }
    }
}