using UnityEngine;

/// <summary>
/// "Core" (trognon) apple variant: falls fast, gives a negative coefficient
/// and a small score penalty when caught, and temporarily slows the
/// player down. Missing it never damages the player's health.
/// </summary>
public class CoreApple : AppleParent
{
    [SerializeField] private int   malusScore    = -1;
    [SerializeField] private float malusSpeed    = 0.5f;
    [SerializeField] private float malusDuration = 3f;
    
    private static float speed;

    protected override float Speed
    {
        get => speed;
        set => speed = value;
    }

    /// <summary>Initializes Core apple-specific stats (speed, coefficient, malus values).</summary>
    protected override void Start()
    {
        base.Start();
        Speed           = 12f;
        attackThreshold = 0f;
        coefficient     = -2;
        damage          = 0;
        malusScore      = -1;
        malusSpeed      = 0.5f;
        malusDuration   = 3f;
    }

    /// <summary>Slows the apple down once it crosses the attack threshold (it starts fast, then eases off).</summary>
    protected override void UpdateSpeed()
    {
        if (transform.position.y < attackThreshold)
            Speed = 4f;
    }

    /// <summary>Applies a negative coefficient, a score penalty, and a temporary slow-down when caught.</summary>
    protected override void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberCatchTronion(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        CatchboyController.Instance.AddScore(malusScore);
        CatchboyController.Instance.editSpeed(malusSpeed, malusDuration);
    }
}