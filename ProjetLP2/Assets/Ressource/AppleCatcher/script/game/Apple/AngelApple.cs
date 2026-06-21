using UnityEngine;

/// <summary>
/// "Angel" apple variant: falls slower than a normal apple, grants
/// the player bonus health when caught instead of points, and
/// never damages the player's health when missed.
/// </summary>
public class AngelApple : AppleParent
{
    [SerializeField] private int bonusHealth = 1;
    private static float speed;

    protected override float Speed
    {
        get => speed;
        set => speed = value;
    }

    protected override void Start()
    {
        base.Start();
        Speed           = 4f;
        attackThreshold = 0f;
        coefficient     = 5;
        damage          = 0;
        bonusHealth     = 1;
    }

    protected override void UpdateSpeed()
    {
        if (transform.position.y < attackThreshold)
            Speed = 8f;
    }

    protected override void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberCatchAngel(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        AppleSpawner.Instance.editHealth(bonusHealth);
    }
}