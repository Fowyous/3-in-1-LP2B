using UnityEngine;

/// <summary>
/// "Angel" apple variant: falls slowly at first then accelerates,
/// grants the player bonus health when caught instead of points,
/// and never damages the player's health when missed.
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

    /// <summary>Initializes Angel apple-specific stats (slow initial speed, high coefficient, no miss penalty).</summary>
    protected override void Start()
    {
        base.Start();
        Speed           = 4f;
        attackThreshold = 0f;
        coefficient     = 5;
        damage          = 0;
        bonusHealth     = 1;
    }

    /// <summary>Accelerates the apple once it crosses the attack threshold (slow descent, then fast finish).</summary>
    protected override void UpdateSpeed()
    {
        if (transform.position.y < attackThreshold)
            Speed = 8f;
    }

    /// <summary>Grants bonus health to the player and applies the combo coefficient when caught. Awards no score points.</summary>
    protected override void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberCatchAngel(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        AppleSpawner.Instance.editHealth(bonusHealth);
    }
}