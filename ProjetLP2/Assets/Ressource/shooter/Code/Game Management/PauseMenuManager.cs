using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
  [Header("UI settings")]
  [SerializeField] private Canvas pauseCanvas;

  [Header("Audio")]
  [SerializeField] private AudioSource MainMusic;
  [SerializeField] private AudioClip TogglePauseAudio;

  private AudioSource audioSource;

  private bool isPaused = false;
  void Start()
  {

    pauseCanvas.gameObject.SetActive(false);//canvas disabled by default

    audioSource = GetComponent<AudioSource>();
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
    MainMusic.Pause();
    audioSource.PlayOneShot(TogglePauseAudio);

    isPaused = true;
    pauseCanvas.gameObject.SetActive(true);

    Time.timeScale = 0f; // Freeze the game  
  }

  public void Resume()
  {
    MainMusic.Play();
    audioSource.PlayOneShot(TogglePauseAudio);

    isPaused = false;
    pauseCanvas.gameObject.SetActive(false);
    Time.timeScale = 1f; // Resume normal time 
  }
}
