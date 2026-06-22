using UnityEngine;

/// <summary>
/// "Simple" brick variant: the most basic brick, destroyed in a single
/// hit. Reuses the base class's collision/heal/damage flow entirely and
/// only adds its own statistic tracking on destruction.
/// </summary>
public class SimpleBrick : BrickParent
{
    protected override void Start()
    {
        base.Start();
        health      = 1;
        pointValue  = 10;
        coefficient = 1;
    }

    /// <summary>Logs the score gained and updates the "simple bricks destroyed" statistic.</summary>
    protected override void OnBrickDestroyed()
    {
        if (!isAesthetic)
        {
            Debug.Log("Points gagnés : " + pointValue);
            GameOverControllerBrickBreaker.editNumberSimple(1);
        }
    }
}