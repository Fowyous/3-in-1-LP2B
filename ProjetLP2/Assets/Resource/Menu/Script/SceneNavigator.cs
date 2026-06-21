using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Central helper for scene transitions used across the whole app: loading
/// a scene asynchronously, and optionally playing a sound (waiting for it
/// to finish) right before the transition.
/// </summary>
public class SceneNavigator : MonoBehaviour
{
    public static SceneNavigator Instance { get; private set; }

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>Loads a scene asynchronously and waits for it to finish.</summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    /// <summary>Plays a sound (waiting for it to finish), then loads a scene asynchronously.</summary>
    public void LoadSceneWithSound(string sceneName, AudioClip clip)
    {
        StartCoroutine(LoadSceneWithSoundRoutine(sceneName, clip));
    }

    /// <summary>Plays a sound and waits for it to finish, if both the source and the clip are available.</summary>
    public void PlaySound(AudioClip clip)
    {
        StartCoroutine(PlaySoundRoutine(clip));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    /// <summary>Plays the sound (waiting for it to finish), then loads the given scene asynchronously.</summary>
    private IEnumerator LoadSceneWithSoundRoutine(string sceneName, AudioClip clip)
    {
        yield return StartCoroutine(PlaySoundRoutine(clip));
        yield return StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator PlaySoundRoutine(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}