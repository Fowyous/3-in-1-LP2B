using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the BrickBreaker "press any key to start" home screen: plays a
/// sound and transitions to the main game scene on the first key press.
/// </summary>
public class HomeControllerBrickBreaker : MonoBehaviour
{
    public AudioClip enterSong;
    private AudioSource audioSource;
    private bool isLoading = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (isLoading) return;

        if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isLoading = true;

            if (audioSource != null && enterSong != null)
            {
                audioSource.PlayOneShot(enterSong);
            }

            StartCoroutine(LoadMainGame());
        }
    }

    /// <summary>Waits for the start sound to finish, then loads the main game scene and disables aesthetic mode.</summary>
    private IEnumerator LoadMainGame()
    {
        if (enterSong != null)
            yield return new WaitForSeconds(enterSong.length);

        AsyncOperation load = SceneManager.LoadSceneAsync("gameBrickBreaker");

        while (!load.isDone)
        {
            yield return null;
        }
        if (SpawnerBall.Instance != null)
            SpawnerBall.Instance.setIsEstetique(false);
    }
}