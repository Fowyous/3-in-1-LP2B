using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerBall : MonoBehaviour
{
    public static SpawnerBall Instance { get; private set; }

    [SerializeField] private GameObject    ballPrefab;
    [SerializeField] private int           maxLivesDefault = 3;
    [SerializeField] private float         respawnDelay    = 2f;
    [SerializeField] private TextMeshProUGUI livesText;        // ✅ Fix 3
    [SerializeField] public  GameObject    paddle;

    private static int maxLives;                               // ✅ Fix 2 : static
    private static int currentLives;

    private List<GameObject>            activeBalls   = new();
    private Dictionary<GameObject, bool> ballLifeCost = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        maxLives     = maxLivesDefault;
        currentLives = maxLives;
        UpdateText();
        SpawnBall();
    }

    void Update()
    {
        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            GameObject ball = activeBalls[i];
            if (ball == null)
            {
                ballLifeCost.TryGetValue(ball, out bool costs);
                ballLifeCost.Remove(ball);
                activeBalls.RemoveAt(i);
                OnBallDestroyed(costs);
            }
        }
    }

    private void OnBallDestroyed(bool countsAsLife)
    {
        if (countsAsLife)
        {
            currentLives--;
            UpdateText();
        }

        if (currentLives <= 0)
            StartCoroutine(LoadGameOver());
        else if (activeBalls.Count == 0)
            StartCoroutine(RespawnWithDelay());
    }

    public IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnBall();
    }

    public void SpawnBall()
    {
        Vector3 spawnPos = new Vector3(
            paddle.transform.position.x,
            paddle.transform.position.y + 3.5f,
            0);

        GameObject ball = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        ball.GetComponent<Ball>().countsAsLife = true;
        activeBalls.Add(ball);
        ballLifeCost[ball] = true;
    }

    public void DuplicateBall()
    {
        GameObject existingBall = GameObject.FindWithTag("Ball");
        if (existingBall == null) return;

        Ball       originalBall  = existingBall.GetComponent<Ball>();
        GameObject newBall       = Instantiate(ballPrefab, existingBall.transform.position, Quaternion.identity);
        Ball       newBallScript = newBall.GetComponent<Ball>();

        float   angle      = 15f * Mathf.Deg2Rad;
        Vector2 dir        = originalBall.direction;
        Vector2 rotatedDir = new Vector2(
            dir.x * Mathf.Cos(angle) - dir.y * Mathf.Sin(angle),
            dir.x * Mathf.Sin(angle) + dir.y * Mathf.Cos(angle));

        newBallScript.SetDirection(rotatedDir);

        bool costLife              = Random.value < 0.5f;
        newBallScript.countsAsLife = costLife;
        activeBalls.Add(newBall);
        ballLifeCost[newBall] = costLife;
    }

    private void UpdateText()
    {
        if (livesText != null)
            livesText.SetText("Vies : " + currentLives);
    }

    private IEnumerator LoadGameOver()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameOverBrikeBreak");
        while (!load.isDone) yield return null;
    }

    public static void healLives(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);
        Instance.UpdateText();
    }

    public static void healMaxLives(int amount)
    {
        maxLives += amount;
        healLives(amount);  
    }
}