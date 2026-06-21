using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
  [Header("UI settings")]
  [SerializeField] private Canvas pauseCanvas;

  private bool isPaused = false;
  void Start()
  {

    pauseCanvas.gameObject.SetActive(false);//canvas disabled by default
  }

  // Update is called once per frame
  void Update()
  {
    if (Keyboard.current.escapeKey.wasPressedThisFrame)
    {
      Debug.Log("escapeKey pressed");
      if (isPaused)
        Resume();
      else
        Pause();
    }

  }

  public void Pause()
  {
    isPaused = true;
    pauseCanvas.gameObject.SetActive(true);
    Time.timeScale = 0f; // Freeze the game  
  }

  public void Resume()
  {
    isPaused = false;
    pauseCanvas.gameObject.SetActive(false);
    Time.timeScale = 1f; // Resume normal time 
  }
}
