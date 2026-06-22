using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Spawns all apple types on a timer and tracks the player's remaining
/// lives. Triggers the Game Over scene when health reaches zero.
/// </summary>
public class AppleSpawner : MonoBehaviour
{
    public static AppleSpawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject[] applePrefabs;

    [Header("UI")]
    public TextMeshPro healthText;
    public TextMeshPro emptyHealthText;
    public TextMeshPro goldenHealthText;
    
    [SerializeField] private bool isAesthetic;

    private static int health;
    private static int healthMax;
    private bool       isGameOver = false;

    private struct SpawnTimer
    {
        public int   AppleIndex;
        public float Timer;
        public float MinInterval;
        public float MaxInterval;
    }

    private SpawnTimer[] spawnTimers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        health    = 3;
        healthMax = health;

        
        spawnTimers = new[]
        {
            new SpawnTimer { AppleIndex = 0, Timer = 0f,                     MinInterval = 0.5f, MaxInterval = 2f  },
            new SpawnTimer { AppleIndex = 1, Timer = Random.Range(0f, 30f),  MinInterval = 20f,  MaxInterval = 60f },
            new SpawnTimer { AppleIndex = 2, Timer = Random.Range(0f, 30f),  MinInterval = 20f,  MaxInterval = 60f },
            new SpawnTimer { AppleIndex = 3, Timer = Random.Range(0f, 30f),  MinInterval = 20f,  MaxInterval = 60f },
            new SpawnTimer { AppleIndex = 4, Timer = Random.Range(0f, 30f),  MinInterval = 20f,  MaxInterval = 60f },
        };

        RefreshHearts();
    }

    private void Update()
    {
        for (int i = 0; i < spawnTimers.Length; i++)
        {
            spawnTimers[i].Timer -= Time.deltaTime;
            if (spawnTimers[i].Timer <= 0f)
            {
                SpawnApple(spawnTimers[i].AppleIndex);
                spawnTimers[i].Timer = Random.Range(spawnTimers[i].MinInterval, spawnTimers[i].MaxInterval);
            }
        }
    }

    /// <summary>Instantiates an apple of the given type at a random X position above the screen.</summary>
    private void SpawnApple(int index)
    {
        GameObject  newApple    = Instantiate(applePrefabs[index]);
        float       newX        = Random.Range(-8f, 8f);
        newApple.transform.position = new Vector3(newX, 7f, 0f);

        AppleParent appleScript = newApple.GetComponent<AppleParent>();
        appleScript.SetAesthetic(isAesthetic);
    }

    /// <summary>Adds (or subtracts) health, refreshes the display, and triggers Game Over if health reaches zero.</summary>
    public void editHealth(int value)
    {
        if (isGameOver) return; 

        health += value;
        RefreshHearts();

        if (health <= 0)
        {
            isGameOver = true;
            SceneNavigator.Instance.LoadScene(SceneNames.APPLE_CATCHER_GAME_OVER);
        }
    }

    /// <summary>
    /// Rebuilds the three-layer heart display:
    ///   emptyHealthText  = healthMax empty slots (background)
    ///   goldenHealthText = health golden hearts   (mid layer, shows bonus lives beyond max)
    ///   healthText       = min(health, healthMax) filled hearts (top layer, covers golden ones up to max)
    /// </summary>
    private void RefreshHearts()
    {
        string filled = "";
        string golden = "";
        string empty  = "";

        for (int i = 0; i < health; i++)
        {
            if (i <= healthMax - 1)
                filled += "<sprite name=\"pixil-frame-0_0\">";

            golden += "<sprite name=\"pixil-frame-0 (7)_0\">";
        }

        for (int i = 0; i < healthMax; i++)
        {
            empty += "<sprite name=\"pixil-frame-0 (2)_0\">";
        }

        healthText.text       = filled;
        goldenHealthText.text = golden;
        emptyHealthText.text  = empty;
    }
}