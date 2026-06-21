using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour
{
    public  AudioClip entrerSong;
    private static AudioSource audioSource; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
        }
        else if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (audioSource != null && entrerSong != null)
            {
                audioSource.PlayOneShot(entrerSong);
            }
            StartCoroutine(LoadMainGame());
        }
    }
    
    private IEnumerator LoadMainGame()
    {
        if (entrerSong != null)
            yield return new WaitForSeconds(entrerSong.length);
        
        AsyncOperation load = SceneManager.LoadSceneAsync("gameBrickBreaker");
        SpawnerBall.Instance.setIsEstetique(false);

        while (!load.isDone)
        {
            yield return null;
        }
    }
}
