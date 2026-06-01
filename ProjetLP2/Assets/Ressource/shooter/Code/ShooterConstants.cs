using UnityEngine;

public static class ShooterConstants
{
  public const string AppName = "MyApplication";
  ///<summary>
  ///here we limit the motion of all movable objects (enemies, player, asteroids ...)
  ///it is like an imaginary wall
  ///</summary>
  public static class GameLimit
  {
    public const float x = 8.2f;
    public const float y = 4.5f;
  }

  public const float Phase1limit = 5.1f;
  public const float Phase2limit = -5.1f;


}
