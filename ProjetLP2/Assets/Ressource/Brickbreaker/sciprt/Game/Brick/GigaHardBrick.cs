using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// "Giga Hard" brick variant: starts with much higher health than a
/// simple brick, visually changes sprite as it takes damage, and
/// awards a large score bonus plus a coefficient boost when destroyed.
/// </summary>
public class GigaHardBrick : BrickParent
{
    [FormerlySerializedAs("Sprite")]
    [SerializeField] private Sprite[] damageSprites;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();
        health         = 6;
        pointValue     = 100;
        coefficient    = 10;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Giga bricks don't use the shared heal-then-damage flow from the
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
    /// brick's sprite to reflect remaining health, and destroys it with
    /// bonus score/coefficient once health reaches zero.
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
            spriteRenderer.sprite = damageSprites[health - 1];
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
}