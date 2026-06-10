using UnityEngine;
using System.Collections;

///<summary>
///Plays the laser charge animation on the ChargeSprite child GameObject.
///Uses Unity's Animator to drive the sprite animation.
///Call PlayChargeEffect() before firing the laser.
///</summary>
public class LaserChargeEffect : MonoBehaviour
{
    [Header("Charge Sprite")]
    [Tooltip("Assign the ChargeSprite child GameObject here.")]
    [SerializeField] private GameObject chargeSprite;

    [Header("Charge Settings")]
    [SerializeField] private float chargeDuration = 1.0f; // Must match WaitForSeconds in LockAndShootRoutine

    private Animator animator;
    private Coroutine activeCharge;

    void Awake()
    {
        if (chargeSprite != null)
        {
            animator = chargeSprite.GetComponent<Animator>();
            chargeSprite.SetActive(false); // Hidden at start
        }
        else
        {
            Debug.LogWarning("LaserChargeEffect: chargeSprite is not assigned!");
        }
    }

    ///<summary>
    ///Shows the charge sprite and plays the animation for chargeDuration seconds, then hides it.
    ///</summary>
    public void PlayChargeEffect()
    {
        if (chargeSprite == null) return;

        if (activeCharge != null)
            StopCoroutine(activeCharge);

        activeCharge = StartCoroutine(ChargeRoutine());
    }

    private IEnumerator ChargeRoutine()
    {
        // Show and play the animation
        chargeSprite.SetActive(true);

        if (animator != null)
            animator.Play(0); // Replays the default state from the beginning

        // Wait for the charge duration
        yield return new WaitForSeconds(chargeDuration);

        // Hide after charge is done
        chargeSprite.SetActive(false);
        activeCharge = null;
    }

    ///<summary>
    ///Immediately hides the charge effect (called if the monster dies mid-charge).
    ///</summary>
    public void StopChargeEffect()
    {
        if (activeCharge != null)
        {
            StopCoroutine(activeCharge);
            activeCharge = null;
        }

        if (chargeSprite != null)
            chargeSprite.SetActive(false);
    }
}