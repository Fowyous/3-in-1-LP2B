///<summary>
///The 5 power-up types available in the game, as defined in the spec:
///rapid fire, speed boost, shield (invincibility), bomb fragment, and heal.
///Collecting 4 Bomb fragments lets the player trigger a special attack
///that wipes out every enemy on screen (see UFO.TriggerSpecialAttack()).
///</summary>
public enum PowerUpType
{
    RapidFire,
    SpeedBoost,
    Shield,
    Bomb,
    Heal
}