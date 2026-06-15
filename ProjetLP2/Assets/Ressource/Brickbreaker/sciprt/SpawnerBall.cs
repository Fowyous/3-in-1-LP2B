using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerBall : MonoBehaviour
{
    public static SpawnerBall Instance { get; private set; }

    [SerializeField] private GameObject  ballPrefab;
    [SerializeField] private int         maxLivesDefault = 3;
    [SerializeField] private float       respawnDelay    = 2f;
    [SerializeField] private TextMeshPro livesText;
    [SerializeField] public  GameObject  paddle;
    [SerializeField] private bool isEstetique;
    public AudioClip loosLifeSong;
    private static AudioSource audioSource;

    private static int maxLives;
    private static int currentLives;

    private List<GameObject>             activeBalls   = new();
    private Dictionary<GameObject, bool> ballLifeCost  = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        maxLives     = maxLivesDefault;
        currentLives = maxLives;
        RefreshHearts();
        SpawnBall(paddle.transform.position.x, paddle.transform.position.y, 0f);
        audioSource = GetComponent<AudioSource>();
    }
    public void NotifyBallDestroyed(GameObject ball, bool countsAsLife)
    {
        activeBalls.Remove(ball);
        ballLifeCost.Remove(ball);
        OnBallDestroyed(countsAsLife);
    }

    private void OnBallDestroyed(bool countsAsLife)
    {
        if (countsAsLife && !isEstetique)
        {
            currentLives--;
            RefreshHearts();
        }

        if (currentLives <= 0)
        {
            StartCoroutine(LoadGameOver());
        }
        else if (activeBalls.Count == 0)
        {
            if (audioSource != null && loosLifeSong != null)
                audioSource.PlayOneShot(loosLifeSong);
            StartCoroutine(RespawnWithDelay());
        }
    }

    public IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnBall(paddle.transform.position.x, paddle.transform.position.y, 0f);
    }

    private void SpawnBall(float positionx, float positiony, float positionz)
    {
        Vector3    spawnPos   = new Vector3(positionx, positiony + 3.5f, positionz);
        GameObject ball       = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        Ball       ballScript = ball.GetComponent<Ball>();
        
        float   angle      = Random.Range(-15f, 15f) * Mathf.Deg2Rad;
        Vector2 initialDir = new Vector2(Mathf.Sin(angle), -1 * Mathf.Cos(angle)).normalized;
        ballScript.SetDirection(initialDir);

        ballScript.countsAsLife = true;
        activeBalls.Add(ball);
        ballLifeCost[ball] = true;
    }
    public bool DuplicateBall()
    {
        if (activeBalls.Count == 0)
        {
            Debug.Log("No active ball to duplicate");
            return false;
        }

        GameObject existingBall  = activeBalls[0]; 
        Ball       originalBall  = existingBall.GetComponent<Ball>();
        GameObject newBall       = Instantiate(ballPrefab, existingBall.transform.position, Quaternion.identity);
        Ball       newBallScript = newBall.GetComponent<Ball>();

        float   angle      = Random.Range(-30f, 30f) * Mathf.Deg2Rad;
        Vector2 dir        = originalBall.direction;
        Vector2 rotatedDir = new Vector2(
            dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle),
            dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle)
        );

        newBallScript.SetDirection(rotatedDir);

        bool costLife              = Random.value < 0.5f;
        newBallScript.countsAsLife = costLife;
        activeBalls.Add(newBall);
        ballLifeCost[newBall] = costLife;

        return costLife;
    }

    private IEnumerator LoadGameOver()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameOverBrikeBreak");
        while (!load.isDone) yield return null;
    }

    public static void healLives(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        Instance.RefreshHearts();
    }

    public static void healMaxLives(int amount)
    {
        maxLives += amount;
        healLives(amount);
    }

    private void RefreshHearts()
    {
        string healthString = "";
        for (int i = 0; i < currentLives; i++)
            healthString += "<sprite name=\"Ball_0\">";
        livesText.text = healthString;
    }
    
    public void setIsEstetique(bool value)
    {
        isEstetique = value;
    }

    public bool getIsEstetique()
    {
        return isEstetique;
    }
}