using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public AudioClip songClique;
    private AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private IEnumerator playSong()
    {
        audioSource.PlayOneShot(songClique);
        if (songClique != null)
            yield return new WaitForSeconds(songClique.length);
    }
    
    public void startAppleCatcher()
    {
        StartCoroutine(loadAppleCatcher());
    }
    
    private IEnumerator loadAppleCatcher()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("homeAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startAppleCatcherRule()
    {
        StartCoroutine(loadAppleCatcherRule());
    }
    
    private IEnumerator loadAppleCatcherRule()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("ruleAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startBrickBreaker()
    {
        StartCoroutine(loadBrickBreaker());
    }
    
    private IEnumerator loadBrickBreaker()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("homeBrickBreaker");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startBrickBreakerRule()
    {
        StartCoroutine(loadBrickBreakerRule());
    }
    
    private IEnumerator loadBrickBreakerRule()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("ruleBrickBreaker");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startMiniUfoAttack()
    {
        StartCoroutine(loadMiniUfoAttack());
    }
    
    private IEnumerator loadMiniUfoAttack()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("Shooter");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void startMiniUfoAttackRule()
    {
        StartCoroutine(loadMiniUfoAttackRule());
    }
    
    private IEnumerator loadMiniUfoAttackRule()
    {
        yield return StartCoroutine(playSong());
        AsyncOperation load = SceneManager.LoadSceneAsync("ruleShooter");
        while (!load.isDone)
        {
            yield return null;
        }
    }
    
    public void quitApplication()
    {
        Application.Quit();
    }
}
