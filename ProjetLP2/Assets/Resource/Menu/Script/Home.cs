using UnityEngine;

/// <summary>
/// Controls the global main menu: navigation to each mini-game's home/rules
/// screen (with a click sound before each transition) and quitting the app.
/// These public methods are meant to be wired directly to UI button OnClick events.
/// </summary>
public class Home : MonoBehaviour
{
    public AudioClip clickSound;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void startAppleCatcher()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.APPLE_CATCHER_HOME, clickSound);
    }

    public void startAppleCatcherRule()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.APPLE_CATCHER_RULE, clickSound);
    }

    public void startBrickBreaker()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.BRICK_BREAKER_HOME, clickSound);
    }

    public void startBrickBreakerRule()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.BRICK_BREAKER_RULE, clickSound);
    }

    public void startMiniUfoAttack()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.MINI_UFO_ATTACK_GAME, clickSound);
    }

    public void startMiniUfoAttackRule()
    {
        SceneNavigator.Instance.LoadSceneWithSound(SceneNames.MINI_UFO_ATTACK_RULE, clickSound);
    }

    public void quitApplication()
    {
        Application.Quit();
    }
}