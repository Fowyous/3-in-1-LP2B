using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Spawns and manages the grid of bricks for one BrickBreaker level.
/// Handles random level generation with configurable spawn probabilities
/// per brick type, difficulty progression over levels, and score/coefficient
/// tracking.
/// </summary>
public class BrickSpawner : MonoBehaviour
{
    public static BrickSpawner Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private GameObject[] blockArray;
    [SerializeField] private TextMeshPro  scoreText;
    [SerializeField] private TextMeshPro  levelText;

    private static int score;
    private static int coefficient;
    private static int level;

    [Header("Audio")]
    public AudioClip pointWinSong;
    private static AudioSource audioSource;

    [Header("Grid Size")]
    [SerializeField] private int rows;
    [SerializeField] private int cols;

    [Header("Block Spacing")]
    [SerializeField] private float blockWidth  = 1.76f;
    [SerializeField] private float blockHeight = 0.96f;
    [SerializeField] private float startX      = -8.5f;
    [SerializeField] private float startY      =  4.5f;
    private float startZ;

    [Header("Spawn Probabilities (out of 100)")]
    [Range(0, 100)] [SerializeField] private int chanceEmpty;
    [Range(0, 100)] [SerializeField] private int chanceSimple;
    [Range(0, 100)] [SerializeField] private int chanceHard;
    [Range(0, 100)] [SerializeField] private int chanceGigaHard;
    [Range(0, 100)] [SerializeField] private int chanceLucky;
    
    private int chanceEmptyBackup;
    private int chanceSimpleBackup;
    private int chanceHardBackup;
    private int chanceGigaHardBackup;
    private int chanceLuckyBackup;

    private bool isAesthetic;

    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool levelCleared = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        level = 1;

        isAesthetic = SpawnerBall.Instance.getIsEstetique();
        if (isAesthetic)
        {
            startZ = 91f;
        }
        else
        {
            startZ = 0f;
            scoreText.SetText("score :\n" + score);
            levelText.SetText("level : " + level);
        }

        SpawnRandomLevel();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (levelCleared) return;
        
        for (int i = activeBlocks.Count - 1; i >= 0; i--)
        {
            if (activeBlocks[i] == null)
                activeBlocks.RemoveAt(i);
        }

        if (activeBlocks.Count == 0)
        {
            levelCleared = true;
            level++;

            if (!isAesthetic)
            {
                levelText.SetText("level : " + level);
            }

            if (level % 2 == 0)
            {
                CacheChances();
            }

            UpdateSpawnChances();
            SpawnerBall.healLives(1);
            SpawnRandomLevel();

            if (level % 2 == 0)
            {
                RestoreChances();
            }

            SpawnerBall.Instance.RespawnBallFree();
        }
    }

    /// <summary>Generates a new random grid of bricks and spawns it.</summary>
    private void SpawnRandomLevel()
    {
        levelCleared = false;

        BrickType[][] grid = GenerateRandomGrid();
        SpawnGrid(grid);
    }

    /// <summary>Rolls a random brick type for every cell of the grid.</summary>
    private BrickType[][] GenerateRandomGrid()
    {
        BrickType[][] grid = new BrickType[rows][];

        for (int row = 0; row < rows; row++)
        {
            grid[row] = new BrickType[cols];
            for (int col = 0; col < cols; col++)
            {
                grid[row][col] = GetRandomCellType();
            }
        }

        return grid;
    }

    /// <summary>Rolls a random brick type for one grid cell, based on the current spawn probabilities.</summary>
    private BrickType GetRandomCellType()
    {
        BrickType[] types   = { BrickType.Empty, BrickType.Simple, BrickType.Hard, BrickType.GigaHard, BrickType.Lucky };
        int[]       chances = { chanceEmpty, chanceSimple, chanceHard, chanceGigaHard, chanceLucky };

        int roll       = Random.Range(0, 100);
        int cumulative = 0;

        for (int i = 0; i < types.Length; i++)
        {
            cumulative += chances[i];
            if (roll < cumulative)
                return types[i];
        }

        return BrickType.Empty;
    }

    /// <summary>Instantiates one brick prefab per non-empty cell of the grid.</summary>
    private void SpawnGrid(BrickType[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                BrickType cellType = grid[row][col];

                if (cellType == BrickType.Empty)
                    continue;

                Vector3    position = new Vector3(startX + col * blockWidth, startY - row * blockHeight, startZ);
                GameObject prefab   = blockArray[(int)cellType];
                GameObject block    = Instantiate(prefab, position, Quaternion.identity);
                activeBlocks.Add(block);
            }
        }
    }

    /// <summary>Saves the current spawn probabilities so they can be restored after a themed level.</summary>
    private void CacheChances()
    {
        chanceEmptyBackup    = chanceEmpty;
        chanceSimpleBackup   = chanceSimple;
        chanceHardBackup     = chanceHard;
        chanceGigaHardBackup = chanceGigaHard;
        chanceLuckyBackup    = chanceLucky;
    }

    /// <summary>Adjusts spawn probabilities to ramp up difficulty as levels progress.</summary>
    private void UpdateSpawnChances()
    {
        if (level % 3 == 0)
        {
            if (level < 12)
            {
                chanceEmpty    -= 5;
                chanceLucky    -= 1;
                chanceSimple   -= 2;
                chanceHard     += 6;
                chanceGigaHard += 2;
            }
            else
            {
                if (chanceLucky > 1)
                {
                    chanceEmpty    -= 2;
                    chanceLucky    -= 1;
                    chanceSimple   -= 2;
                    chanceHard     += 4;
                    chanceGigaHard += 1;
                }
                else
                {
                    if (chanceSimple == 0 || chanceEmpty <= 4)
                    {
                        chanceHard     -= 2;
                        chanceGigaHard += 2;
                        if (chanceHard <= 0)
                        {
                            chanceHard = 0;
                        }
                    }
                    else
                    {
                        chanceEmpty    -= 2;
                        chanceSimple   -= 2;
                        chanceHard     += 3;
                        chanceGigaHard += 1;
                    }
                }
            }
        }
        else if (level % 2 == 0)
        {
            int roll = Random.Range(0, 5);
            switch (roll)
            {
                case 0:
                    chanceEmpty = 100; chanceSimple = 0; chanceHard = 0; chanceGigaHard = 0; chanceLucky = 0;
                    break;
                case 1:
                    chanceEmpty = 0; chanceSimple = 100; chanceHard = 0; chanceGigaHard = 0; chanceLucky = 0;
                    break;
                case 2:
                    chanceEmpty = 0; chanceSimple = 0; chanceHard = 100; chanceGigaHard = 0; chanceLucky = 0;
                    break;
                case 3:
                    chanceEmpty = 0; chanceSimple = 0; chanceHard = 0; chanceGigaHard = 100; chanceLucky = 0;
                    break;
                case 4:
                    chanceEmpty = 0; chanceSimple = 0; chanceHard = 0; chanceGigaHard = 0; chanceLucky = 100;
                    break;
            }
        }
    }

    /// <summary>Restores the spawn probabilities saved by CacheChances().</summary>
    private void RestoreChances()
    {
        chanceEmpty    = chanceEmptyBackup;
        chanceSimple   = chanceSimpleBackup;
        chanceHard     = chanceHardBackup;
        chanceGigaHard = chanceGigaHardBackup;
        chanceLucky    = chanceLuckyBackup;
    }

    public static void setCoefficient(int coef)
    {
        if (coef == 0)
        {
            coefficient = 0;
        }
        else
        {
            coefficient += coef;
        }
    }

    public void AddScore(int points)
    {
        if (!isAesthetic)
        {
            score += points * coefficient;
            scoreText.SetText($"{score} * {coefficient} points");
            if (audioSource != null && pointWinSong != null)
            {
                audioSource.PlayOneShot(pointWinSong);
            }
        }
    }

    /// <summary>Destroys all remaining blocks; useful on game over.</summary>
    public void ClearLevel()
    {
        foreach (GameObject block in activeBlocks)
        {
            if (block != null) Destroy(block);
        }
        activeBlocks.Clear();
    }

    public static int GetScore()
    {
        return score;
    }

    public static void SetScore(int newScore)
    {
        score = newScore;
    }

    public static int GetLevel()
    {
        return level;
    }
}