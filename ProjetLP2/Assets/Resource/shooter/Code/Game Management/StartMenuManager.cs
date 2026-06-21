using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class StartMenuManager : MonoBehaviour
{
  [Header("UI settings")]
  [SerializeField] private float fadeDuration = 3.0f; // Time to fade in/out
  [SerializeField] private TextMeshProUGUI countdownText; // Use Text for UI.Text

  [SerializeField] private Canvas StartMenuCanvas;
  [SerializeField] private float holdDuration = 0.5f;
  private bool keyPressed = false;
  private Color originalColor;
  void Start()
  {
    Time.timeScale = 0; // Pause the game


    originalColor = countdownText.color;
    StartMenuCanvas.gameObject.SetActive(true);//show menu UI
    StartCoroutine(ReadySetGo());


  }

  void Update()
  {
    // Check for keypress while time is paused  
    if (Keyboard.current.anyKey.wasPressedThisFrame)
    {
      keyPressed = true;
      Debug.Log("StartMenuManager : key pressed");
    }
  }



  IEnumerator ReadySetGo()
  {
    // Ready    
    countdownText.text = "Ready";
    yield return StartCoroutine(FadeTo(0f, 1f, fadeDuration));
    yield return new WaitForSecondsRealtime(holdDuration);
    yield return StartCoroutine(FadeTo(1f, 0f, fadeDuration));

    // Set    
    countdownText.text = "Set";
    yield return StartCoroutine(FadeTo(0f, 1f, fadeDuration));
    yield return new WaitForSecondsRealtime(holdDuration);
    yield return StartCoroutine(FadeTo(1f, 0f, fadeDuration));

    // Go!      
    countdownText.text = "Go!";
    yield return StartCoroutine(FadeTo(0f, 1f, fadeDuration));
    yield return new WaitForSecondsRealtime(holdDuration);
    yield return StartCoroutine(FadeTo(1f, 0f, fadeDuration));

    // Resume game     
    StartMenuCanvas.gameObject.SetActive(false);
    Time.timeScale = 1f;
  }
  IEnumerator FadeBlink()
  {
    while (!keyPressed)
    {
      yield return StartCoroutine(FadeTo(originalColor.a, 0f, fadeDuration)); // Fade out 
      yield return StartCoroutine(FadeTo(0f, originalColor.a, fadeDuration)); // Fade in 
    }


    Debug.Log("StartMenuManager : game start");
    // Resume the game    
    Time.timeScale = 1f;
    StartMenuCanvas.gameObject.SetActive(false);//hide menu UI


  }
  IEnumerator FadeTo(float startAlpha, float targetAlpha, float duration)
  {
    float elapsed = 0f;
    Color newColor = originalColor;
    while (elapsed < duration)
    {
      elapsed += Time.unscaledDeltaTime;
      newColor.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration); countdownText.color = newColor;
      yield return null;
    }
    newColor.a = targetAlpha;
    countdownText.color = newColor;
  }

}
