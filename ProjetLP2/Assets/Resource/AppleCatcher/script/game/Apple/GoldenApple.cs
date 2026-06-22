using UnityEngine;

/// <summary>
/// "Golden" apple variant: falls slowly at first then accelerates,
/// awards bonus points when caught, and temporarily boosts the player's
/// catching speed. Missing it never damages the player's health.
/// </summary>
public class GoldenApple : AppleParent
{
    [SerializeField] private int bonusScore = 2;
    [SerializeField] private float bonusSpeed = 1.5f;
    [SerializeField] private float bonusDuration = 20f;
    
    private static float speed;

    protected override float Speed
    {
        get => speed;
        set => speed = value;
    }

    /// <summary>Initializes Golden apple-specific stats (slow initial speed, high coefficient, no miss penalty).</summary>
    protected override void Start()
    {
        base.Start();
        Speed           = 4f;
        attackThreshold = 0f;
        coefficient     = 3;
        damage          = 0;
        bonusScore      = 2;
        bonusSpeed      = 1.5f;
        bonusDuration   = 20f;
    }

    /// <summary>Accelerates the apple once it crosses the attack threshold (slow descent, then fast finish).</summary>
    protected override void UpdateSpeed()
    {
        if (transform.position.y < attackThreshold)
            Speed = 8f;
    }

    /// <summary>Awards bonus points and grants a temporary catching-speed boost when caught.</summary>
    protected override void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberCatchGolden(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        CatchboyController.Instance.AddScore(bonusScore);
        CatchboyController.Instance.editSpeed(bonusSpeed, bonusDuration);
    }
}