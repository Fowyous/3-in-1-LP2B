using UnityEngine;

public static class ShooterConstants
{
    public const string AppName = "MyApplication";

    ///<summary>
    ///Limits the motion of all movable objects (enemies, player, asteroids...).
    ///It acts like an invisible wall.
    ///The top limit (yTop) is reduced compared to yBottom to account for the
    ///HUD Canvas overlay occupying the top ~20% of the screen, so gameplay
    ///objects never visually overlap with the UI.
    ///</summary>
    public static class GameLimit
    {
        public const float x = 8.2f;

        // Kept for backward compatibility with scripts using ShooterConstants.GameLimit.y
        public const float y = yBottom;

        ///<summary>Upper Y limit, reduced to leave room for the HUD at the top of the screen.</summary>
        public const float yTop = 2.65f;

        ///<summary>Lower Y limit, unchanged (no UI element at the bottom).</summary>
        public const float yBottom = 4.5f;
    }

    public const float Phase1limit = 5.1f;
    public const float Phase2limit = -5.1f;
}