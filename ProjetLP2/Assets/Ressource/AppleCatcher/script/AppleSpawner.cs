using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class AppleSpawner : MonoBehaviour
{
    public GameObject[] applePrefab;

    private float spawnTimer;
    private float spawnTimerBonus1;
    private float spawnTimerBonus2;
    private float spawnTimerMalus1;
    private float spawnTimerMalus2;
    private static int health;
    private static int healthMax;
    public bool isEsthetique;
    public TextMeshPro Health;
    public TextMeshPro EmptyHeath;

    public static AppleSpawner Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        spawnTimer = 0f;
        spawnTimerBonus1 = Random.Range(0f, 30f);
        spawnTimerBonus2 = Random.Range(0f, 30f);
        spawnTimerMalus1 = Random.Range(0f, 30f);
        spawnTimerMalus2 = Random.Range(0f, 30f);
        health = 3;
        healthMax = health;
        RefreshHearts();
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnApple(0);
            spawnTimer = Random.Range(0.5f, 2f);
        }
        spawnTimerBonus1 -= Time.deltaTime;
        if (spawnTimerBonus1 <= 0)
        {
            spawnApple(1);
            spawnTimerBonus1 = Random.Range(20f, 60f); 
        }
        spawnTimerBonus2 -= Time.deltaTime;
        if (spawnTimerBonus2 <= 0)
        {
            spawnApple(2);
            spawnTimerBonus2 = Random.Range(20f, 60f);
        }
        spawnTimerMalus1 -= Time.deltaTime;
        if (spawnTimerMalus1 <= 0)
        {
            spawnApple(3);
            spawnTimerMalus1 = Random.Range(20f, 60f);
        }
        spawnTimerMalus2 -= Time.deltaTime;
        if (spawnTimerMalus2 <= 0)
        {
            spawnApple(4);
            spawnTimerMalus2 = Random.Range(20f, 60f);
        }
    }
    
    private void spawnApple(int index)
    {
        GameObject newApple = Instantiate(applePrefab[index]);
        float newX = Random.Range(-8f, 8f);
        newApple.transform.position = new Vector3(newX, 7f, 0f);
        
        AppleScript appleScript = newApple.GetComponent<AppleScript>();
        appleScript.setIsEsthetique(isEsthetique);
    }
    
    public void editHealth(int value)
    {
        health += value;
        RefreshHearts();

        if (health <= 0)
        {
            StartCoroutine(LoadEndScene());
        }
    }

    private IEnumerator LoadEndScene()
    {
        AsyncOperation load = SceneManager.LoadSceneAsync("GameOverAppleCatcher");
        while (!load.isDone)
        {
            yield return null;
        }
    }

    private void RefreshHearts()
    {
        string healthString = "";
        string EmptyHeathString = "";
        for (int i = 0; i < health; i++)
        {
            healthString += "<sprite name=\"pixil-frame-0_0\">";
        }
        Health.text = healthString;
        for (int j = 0; j < healthMax; j++)
        {
            EmptyHeathString += "<sprite name=\"pixil-frame-0 (2)_0\">";
        }
        EmptyHeath.text = EmptyHeathString;
    }
}