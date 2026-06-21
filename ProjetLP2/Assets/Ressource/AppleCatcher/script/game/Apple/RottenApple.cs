using UnityEngine;

/// <summary>
/// "Rotten" apple variant: falls fast, gives a negative coefficient
/// when caught, and temporarily inverts the player's controls instead
/// of granting points. Missing it never damages the player's health.
/// </summary>
public class RottenApple : AppleParent
{
    [SerializeField] private float malusDuration = 5f;
    private static float speed;

    protected override float Speed
    {
        get => speed;
        set => speed = value;
    }

    /// <summary>Initializes Rotten apple-specific stats (speed, coefficient, malus duration).</summary>
    protected override void Start()
    {
        base.Start();
        Speed           = 12f;
        attackThreshold = 0f;
        coefficient     = -3;
        damage          = 0;
        malusDuration   = 5f;
    }

    /// <summary>Slows the apple down once it crosses the attack threshold (it starts fast, then eases off).</summary>
    protected override void UpdateSpeed()
    {
        if (transform.position.y < attackThreshold)
            Speed = 4f;
    }

    /// <summary>Applies a negative coefficient and temporarily inverts the player's controls when caught.</summary>
    protected override void OnCaught()
    {
        GameOverControllerAppleCatcher.editNumberCatchPourrie(1);
        CatchboyController.Instance.editCoefficient(coefficient);
        CatchboyController.Instance.ApplyInvertedControls(malusDuration);
    }
}