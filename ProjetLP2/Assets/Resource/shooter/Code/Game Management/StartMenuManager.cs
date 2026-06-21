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
