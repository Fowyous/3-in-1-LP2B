using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AppleSpawner : MonoBehaviour
{
    public GameObject[] applePrefab;

    private float spawnTimer;
    private static int health = 3;
    public TextMeshPro  healthText;
    public bool isEsthetique;

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
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void spawnApple()
    {
        int i = Random.Range(0, applePrefab.Length);
        GameObject newApple = Instantiate(applePrefab[i]);
        float newX = Random.Range(-8f, 8f);
        newApple.transform.position = new Vector3(newX, 7f, 0f);
        
        AppleScript appleScript = newApple.GetComponent<AppleScript>();
        appleScript.setIsEsthetique(isEsthetique);
    }
    
    public void editHealth(int value)
    {
        health += value;
        healthText.SetText(health + ": health");

        if (health <= 0)
        {
            StartCoroutine(LoadEndScene());
        }
    }

    private IEnumerator LoadEndScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameOver");
        while (!load.isDone)
        {
            yield return null;
        }
    }
}