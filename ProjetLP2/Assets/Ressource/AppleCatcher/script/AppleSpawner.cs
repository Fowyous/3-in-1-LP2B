using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AppleSpawner : MonoBehaviour
{
    public GameObject applePrefab;

    private float spawnTimer;
    private int health = 3;
    public TextMeshPro  healthText;

    public static AppleSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawnTimer = 0f;
        healthText.SetText(health + ": health");
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnApple();
            spawnTimer = Random.Range(0.5f, 2f);
        }
    }

    private void spawnApple()
    {
        GameObject newApple = Instantiate(applePrefab);
        float newX = Random.Range(-8f, 8f);
        newApple.transform.position = new Vector3(newX, 7f, 0f);
    }

    public void LoseHealth()
    {
        health -= 1;
        healthText.SetText(health + ": health");

        if (health <= 0)
        {
            StartCoroutine(LoadEndScene());
        }
    }

    private IEnumerator LoadEndScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("end");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}