using System.Collections;
using UnityEngine;

///<summary>
///should make a circle expand after the special attack that detonates all enemies
///expands a circle across the whose screen for an explosion effect
///</summary>
public class CircleExpander : MonoBehaviour
{
  [SerializeField] private GameObject circlePrefab;
  [SerializeField] private float expandDuration = 1.5f;  // Time to expand in seconds
  [SerializeField] private float displayDuration = 2f;   // Time to keep visible in seconds

  private GameObject circleInstance;
  private Transform circleTransform;
  private Vector3 initialScale = Vector3.one * 0.1f;  // Start small

  // Call this method to trigger the circle expansion
  public void ActivateCircle()
  {
    // Instantiate the circle as a child of the player
    circleInstance = Instantiate(circlePrefab, transform);
    circleTransform = circleInstance.transform;

    // Reset position and initial scale
    circleTransform.localPosition = circleTransform.localPosition = new Vector3(0, 0, -9.0f);  // Move it in front
    circleTransform.localScale = initialScale;

    // Make sure it's enabled
    circleInstance.SetActive(true);

    // Start the expansion and fade coroutine
    StartCoroutine(ExpandAndDisable());
  }

  private IEnumerator ExpandAndDisable()
  {
    float elapsedTime = 0f;

    // Expand the circle to cover the screen
    while (elapsedTime < expandDuration)
    {
      elapsedTime += Time.deltaTime;
      float progress = elapsedTime / expandDuration;

      // Calculate the scale needed to cover the screen
      // Adjust this value based on your camera setup (typically 50-100)
      float targetScale = 100f * progress;
      circleTransform.localScale = new Vector3(targetScale, targetScale, 1f);

      yield return null;
    }

    // Keep it visible for the specified duration
    yield return new WaitForSeconds(displayDuration);

    // Disable the circle
    circleInstance.SetActive(false);
    // Delete the circle
    DestroyCircle();
  }

  // Destroy the circle instance when you're done with it completely
  public void DestroyCircle()
  {
    if (circleInstance != null)
    {
      Destroy(circleInstance);
    }
  }
}

