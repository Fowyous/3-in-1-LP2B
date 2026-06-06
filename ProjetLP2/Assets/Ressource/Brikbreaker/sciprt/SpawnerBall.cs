using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerBall : MonoBehaviour
{
    public static SpawnerBall Instance { get; private set; }

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private TextMeshPro livesText;
    [SerializeField] public GameObject paddle;

    private static int currentLives;
    private List<GameObject> activeBalls = new List<GameObject>();
    private Dictionary<GameObject, bool> ballLifeCost = new Dictionary<GameObject, bool>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
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
                bool costs = ballLifeCost.TryGetValue(ball, out bool val) && val;
                ballLifeCost.Remove(ball);
                activeBalls.RemoveAt(i);
                OnBallDestroyed(costs);
            }
        }
    }

    /// <summary>Appelé automatiquement depuis Update quand une balle est détruite.</summary>
    private void OnBallDestroyed(bool countsAsLife)
    {
        if (countsAsLife)
        {
            currentLives--;
            UpdateText();
        }

        if (currentLives <= 0)
        {
            StartCoroutine(LoadGameOver());
        }
        else if (activeBalls.Count == 0)
        {
            StartCoroutine(RespawnWithDelay());
        }
    }

    /// <summary>Spawn une balle avec un délai configurable.</summary>
    public IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnBall();
    }

    /// <summary>Spawn une balle normale au-dessus du paddle.</summary>
    public void SpawnBall()
    {
        float paddleX = paddle.transform.position.x;
        float paddleY = paddle.transform.position.y + 3.5f;
        Vector3 spawnPosition = new Vector3(paddleX, paddleY, 0);

        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        ball.GetComponent<Ball>().countsAsLife = true;

        activeBalls.Add(ball);
        ballLifeCost[ball] = true;
    }

    /// <summary>Duplique la balle existante avec une direction légèrement déviée.</summary>
    public void DuplicateBall()
    {
        GameObject existingBall = GameObject.FindWithTag("Ball");
        if (existingBall == null) return;

        GameObject newBall = Instantiate(ballPrefab,
            existingBall.transform.position,
            Quaternion.identity);

        // Dévie la nouvelle balle de 15° par rapport à la balle originale
        Rigidbody2D rb = newBall.GetComponent<Rigidbody2D>();
        Vector2 originalVelocity = existingBall.GetComponent<Rigidbody2D>().linearVelocity;
        float angle = 15f * Mathf.Deg2Rad;
        rb.linearVelocity = new Vector2(
            originalVelocity.x * Mathf.Cos(angle) - originalVelocity.y * Mathf.Sin(angle),
            originalVelocity.x * Mathf.Sin(angle) + originalVelocity.y * Mathf.Cos(angle));

        // 50% de chance que perdre cette balle coûte une vie
        bool costLife = Random.value < 0.5f;
        newBall.GetComponent<Ball>().countsAsLife = costLife;

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
        while (!load.isDone)
        {
            yield return null;
        }
    }

    public static void healLives(int amount)
    {
        currentLives += amount;
    }

    public static void healMaxLives(int amount)
    {
        currentLives += amount;
    }
}