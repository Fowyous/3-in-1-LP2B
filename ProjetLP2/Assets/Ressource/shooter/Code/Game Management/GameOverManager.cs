using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
  public static GameOverManager Instance { get; private set; }

  [SerializeField] private Canvas gameOverCanvas;
  [SerializeField] private RawImage backgroundImage;

  private Texture2D screenshotTexture;

  // making sure there is only one instance of game over manager
  private void Awake()
  {
    Debug.Log("game over manager awake");
    if (Instance == null)
    {
      Instance = this;
      Debug.Log("GameOverManager Singleton initialized successfully");
    }
    else
    {
      Debug.LogWarning("Multiple GameOverManager instances detected!");
    }
    gameOverCanvas.gameObject.SetActive(false);
  }

  private System.Collections.IEnumerator CaptureScreenAndShowGameOver()
  {
    // Wait for end of frame to ensure everything is rendered
    yield return new WaitForEndOfFrame();

    // Capture screenshot
    screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();

    if (!screenshotTexture)
    {
      Debug.LogError("backgroundImage not set");
    }
    // Apply to background image
    backgroundImage.texture = screenshotTexture;

    // Show game over canvas
    gameOverCanvas.gameObject.SetActive(true);
  }

  public void TriggerGameOver()
  {
    Debug.Log("game over triggered");
    // Pause the game
    Time.timeScale = 0f;

    // Capture the screen. We start coroutine so that the method spans over multiple frames because we are waiting for frame to end in this method
    StartCoroutine(CaptureScreenAndShowGameOver());
  }


  public void RestartGame()
  {
    Time.timeScale = 1f; // Resume time
    UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
    );
  }

  public void QuitGame()
  {
    Time.timeScale = 1f; // Resume time before quitting
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}

