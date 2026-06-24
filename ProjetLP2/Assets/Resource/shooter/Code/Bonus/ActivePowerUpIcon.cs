using UnityEngine;
using UnityEngine.UI;
using System.Collections;

///<summary>
///A single HUD icon representing one currently active timed power-up
///(RapidFire, SpeedBoost, or Shield). Shows the power-up sprite with a
///radial "countdown" overlay that empties as the effect's remaining
///time runs out.
///
///SETUP IN UNITY (for the prefab):
///1. Create a GameObject with an Image component (the power-up icon/background).
///2. Add a child Image: set its Image Type to "Filled", Fill Method to "Radial 360",
///   Fill Origin to "Top", and tick "Clockwise" (or not, your preference).
///   Tint it however you like (e.g. semi-transparent white/dark overlay).
///3. Add this script to the root GameObject and assign "Timer Overlay" to that child Image.
///</summary>
public class ActivePowerUpIcon : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The radial Image (Image Type = Filled, Fill Method = Radial 360) used as a countdown overlay.")]
    [SerializeField] private Image timerOverlay;

    private Coroutine countdownRoutine;

    ///<summary>
    ///Starts (or restarts) the countdown visual for the given duration.
    ///The radial overlay starts full (fillAmount = 1) and empties to 0
    ///as time runs out, matching the power-up's remaining duration.
    ///</summary>
    public void StartCountdown(float duration)
    {
        if (countdownRoutine != null) StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(CountdownRoutine(duration));
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        float elapsed = 0f;

        if (timerOverlay != null) timerOverlay.fillAmount = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            if (timerOverlay != null)
                timerOverlay.fillAmount = 1f - Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        if (timerOverlay != null) timerOverlay.fillAmount = 0f;
    }
}
