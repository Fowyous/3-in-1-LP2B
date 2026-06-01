using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private TextMeshPro livesText;

    private static readonly Vector3 SpawnPosition = new Vector3(0f, -5f, 0f);

    private int currentLives;
    private List<GameObject> activeBalls = new List<GameObject>();

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
            if (activeBalls[i] == null)
            {
                activeBalls.RemoveAt(i);
                OnBallDestroyed();
            }
        }
    }

    /// <summary>Appelé automatiquement depuis Update quand une balle est null.</summary>
    private void OnBallDestroyed()
    {
        currentLives--;
        UpdateText();

        if (currentLives <= 0)
        {
            StartCoroutine(LoadGameOver());
        }
        else
        {
            StartCoroutine(RespawnWithDelay());
        }
    }

    /// <summary>Spawn une balle avec un délai configurable.</summary>
    private IEnumerator RespawnWithDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnBall();
    }

    public void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, SpawnPosition, Quaternion.identity);
        activeBalls.Add(ball);
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
}