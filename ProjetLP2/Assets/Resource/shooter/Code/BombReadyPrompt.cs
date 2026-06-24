using UnityEngine;
using TMPro;
using System.Collections;

///<summary>
///Shows a "Press [T] to use your bomb" message in the HUD whenever the player
///has collected enough Bomb fragments to trigger the special attack, and
///hides it otherwise.
///
///SETUP IN UNITY:
///1. Create a TextMeshPro - Text (UI) element in your HUD Canvas, e.g. "BombReadyText".
///   Set its text to something like "Press [T] to use your bomb!".
///2. Create an empty GameObject (or reuse the text's GameObject) and add this script to it.
///3. Assign "Prompt Text" with that TextMeshPro element.
///4. Assign "Player" with the UFO GameObject, or leave empty to auto-find it.
///</summary>
public class BombReadyPrompt : MonoBehaviour
{
  [Header("References")]
  [Tooltip("Leave empty to auto-find the UFO in the scene.")]
  [SerializeField] private UFO player;

  [Tooltip("The TextMeshPro element showing the 'Press [T]' message.")]
  [SerializeField] private TextMeshProUGUI promptText;

  [Header("Blinking Settings")]
  [SerializeField] private float blinkSpeed = 0.5f; // Time between blinks    
  [SerializeField] private float minAlpha = 0.3f; // Minimum transparency (0-1)
  private Coroutine blinkCoroutine;

  void Start()
  {
    if (player == null)
    {
      player = Object.FindAnyObjectByType<UFO>();
      if (player == null)
      {
        Debug.LogError("BombReadyPrompt: No UFO found in the scene!");
        return;
      }
    }

    if (promptText == null)
    {
      Debug.LogError("BombReadyPrompt: promptText is not assigned!");
      return;
    }

    player.OnBombCountChanged += UpdatePrompt;

    // Start hidden until we know the current state
    promptText.gameObject.SetActive(false);
  }

  void OnDestroy()
  {
    if (player != null)
      player.OnBombCountChanged -= UpdatePrompt;
  }

  ///<summary> 
  ///Shows the prompt with blinking effect when the player has enough bombs.
  ///Called automatically whenever UFO.OnBombCountChanged fires.    
  ///</summary>
  private void UpdatePrompt(int currentCount, int required)
  {
    bool isReady = currentCount >= required;
    if (isReady)
    {
      promptText.gameObject.SetActive(true);
      // Stop any existing blink coroutine and start a new one            
      if (blinkCoroutine != null)
        StopCoroutine(blinkCoroutine);
      blinkCoroutine = StartCoroutine(BlinkText());
    }
    else
    {
      promptText.gameObject.SetActive(false);
      // Stop blinking            
      if (blinkCoroutine != null)
      {
        StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
      }
    }
  }

  ///<summary>
  ///Coroutine that makes the text blink by changing its alpha.  
  ///</summary>   
  private IEnumerator BlinkText()
  {
    while (true)
    {
      // Fade to fully opaque
      yield return StartCoroutine(FadeTextAlpha(minAlpha, 1f, blinkSpeed));
      // Fade to minimum alpha    
      yield return StartCoroutine(FadeTextAlpha(1f, minAlpha, blinkSpeed));
    }
  }
  ///<summary>   
  ///Smoothly fades the text alpha between two values.    
  ///</summary> 
  private IEnumerator FadeTextAlpha(float fromAlpha, float toAlpha, float duration)
  {
    float elapsed = 0f;
    Color textColor = promptText.color;
    while (elapsed < duration)
    {
      elapsed += Time.deltaTime;
      textColor.a = Mathf.Lerp(fromAlpha, toAlpha, elapsed / duration);
      promptText.color = textColor;
      yield return null;
    }
    // Ensure final alpha is set exactly        
    textColor.a = toAlpha;
    promptText.color = textColor;
  }
}

