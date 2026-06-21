using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns and manages the player's ball(s) in BrickBreaker: initial spawn,
/// respawn after a loss, life tracking, and ball duplication.
/// </summary>
public class SpawnerBall : MonoBehaviour
{
    public static SpawnerBall Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject  ballPrefab;
    [SerializeField] private TextMeshPro livesText;
    public GameObject paddle;

    [Header("Stats")]
    [SerializeField] private int   maxLivesDefault;
    [SerializeField] private float respawnDelay;
    
    [SerializeField] private bool isAesthetic;

    [Header("Audio")]
    public AudioClip loseLifeSong;
    private static AudioSource audioSource;

    private float startZ;
    private bool  isRespawning = false;

    private static int maxLives;
    private static int currentLives;

    private List<GameObject> activeBalls = new();
    private Dictionary<GameObject, bool> ballLifeCost = new(); 

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        maxLives     = maxLivesDefault;
        currentLives = maxLives;
        audioSource  = GetComponent<AudioSource>();

        if (isAesthetic)
        {
            startZ = 91f;
        }
        else
        {
            startZ = 0f;
            RefreshHearts();
        }

        SpawnBall(paddle.transform.position.x, paddle.transform.position.y, startZ);
    }

    /// <summary>Called by a ball when it gets destroyed, so this spawner can stop tracking it and react accordingly.</summary>
    public void NotifyBallDestroyed(GameObject ball, bool countsAsLife)
    {
        activeBalls.Remove(ball);
        ballLifeCost.Remove(ball);
        OnBallDestroyed(countsAsLife);
    }

    /// <summary>Handles a life loss (if applicable), then either triggers Game Over or schedules a respawn.</summary>
    private void OnBallDestroyed(bool countsAsLife)
    {
        if (countsAsLife && !isAesthetic)
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
            if (audioSource != null && loseLifeSong != null)
                audioSource.PlayOneShot(loseLifeSong);

            if (!isRespawning)
                StartCoroutine(RespawnWithDelay());
        }
    }

    /// <summary>Waits respawnDelay seconds, then spawns a fresh ball above the paddle.</summary>
    public IEnumerator RespawnWithDelay()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);
        SpawnBall(paddle.transform.position.x, paddle.transform.position.y, startZ);
        isRespawning = false;
    }

    /// <summary>Instantiates a new ball at the given position with a randomized initial launch angle.</summary>
    private void SpawnBall(float positionX, float positionY, float positionZ)
    {
        Vector3    spawnPos   = new Vector3(positionX, positionY + 3.5f, positionZ);
        GameObject ball       = Instantiate(ballPrefab, spawnPos, Quaternion.identity);
        Ball       ballScript = ball.GetComponent<Ball>();

        float   angle      = Random.Range(-15f, 15f) * Mathf.Deg2Rad;
        Vector2 initialDir = new Vector2(Mathf.Sin(angle), -1 * Mathf.Cos(angle)).normalized;
        ballScript.SetDirection(initialDir);

        ballScript.countsAsLife = true;
        activeBalls.Add(ball);
        ballLifeCost[ball] = true;
    }

    /// <summary>Spawns a second ball next to an existing one, splitting off at a randomized angle. 50% chance it costs a life if lost.</summary>
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
        AsyncOperation load = SceneManager.LoadSceneAsync("gameOverBrickBreaker");
        while (!load.isDone) yield return null;
    }

    /// <summary>Restores up to "amount" lives, capped at maxLives.</summary>
    public static void healLives(int amount)
    {
        currentLives = Mathf.Min(currentLives + amount, maxLives);

        if (!Instance.isAesthetic)
            Instance.RefreshHearts();
    }

    /// <summary>Increases the maximum life count and immediately grants the same amount as bonus lives.</summary>
    public static void healMaxLives(int amount)
    {
        maxLives += amount;
        healLives(amount);
    }

    /// <summary>Rebuilds the lives display, wrapping to a new line every 4 icons.</summary>
    private void RefreshHearts()
    {
        string healthString = "";
        for (int i = 1; i < currentLives + 1; i++)
        {
            healthString += "<sprite name=\"Ball_0\">";
            if (i % 4 == 0)
            {
                healthString += "\n";
            }
        }
        livesText.text = healthString;
    }

    public void setIsEstetique(bool value)
    {
        isAesthetic = value;
    }

    public bool getIsEstetique()
    {
        return isAesthetic;
    }

    /// <summary>Clears all active balls without costing a life, then respawns one fresh ball (used when a level is cleared mid-flight).</summary>
    public void RespawnBallFree()
    {
        if (isRespawning) return;

        for (int i = activeBalls.Count - 1; i >= 0; i--)
        {
            GameObject ball = activeBalls[i];
            activeBalls.RemoveAt(i);
            ballLifeCost.Remove(ball);
            Destroy(ball);
        }

        isRespawning = true;
        StartCoroutine(RespawnWithDelay());
    }
}