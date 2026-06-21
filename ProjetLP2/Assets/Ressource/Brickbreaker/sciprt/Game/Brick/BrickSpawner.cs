using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawner  : MonoBehaviour 
{
    public static BrickSpawner Instance { get; private set; }

    private const int EMPTY  = -1;
    private const int SIMPLE = 0;
    private const int HARD   = 1;
    private const int GIGA_HARD = 2;
    private const int LUCKY  = 3;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] blockArray;
    [SerializeField]  private TextMeshPro scoreText;
    [SerializeField] private TextMeshPro levelText;
    private static int score;
    private static int coefficient;
    
    [Header("Song")]
    [SerializeField] public AudioClip PointWingSong;
    private static AudioSource audioSource; 

    [Header("Taille de la grille")]
    [SerializeField] private int rows;
    [SerializeField] private int cols;

    [Header("Espacement des blocs")]
    [SerializeField] private float blockWidth  = 1.76f;
    [SerializeField] private float blockHeight = 0.96f;
    [SerializeField] private float startX      = -8.5f;
    [SerializeField] private float startY      =  4.5f;
    private float startZ;

    [Header("Probabilités sur 100")]
    [Range(0, 100)] [SerializeField] private int chanceEmpty;
    [Range(0, 100)]  private int chanceEmptyTemp;
    [Range(0, 100)] [SerializeField] private int chanceSimple;
    [Range(0, 100)] private int chanceSimpleTemp;
    [Range(0, 100)] [SerializeField] private int chanceHard;
    [Range(0, 100)] private int chanceHardTemp;
    [Range(0, 100)] [SerializeField] private int chanceGigaHard;
    [Range(0, 100)] private int chanceGigaHardTemp;
    [Range(0, 100)] [SerializeField] private int chanceLucky;
    [Range(0, 100)] private int chanceLuckyTemp;
    private static int level;
    
    private bool isEstetique;
    
    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool levelCleared = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        score = 0;
        level = 0;
        
        isEstetique = SpawnerBall.Instance.getIsEstetique();
        if (isEstetique)
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

    void Update()
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
            if (!isEstetique)
            {
                levelText.SetText("level : " + level);
            }
            if (level % 5 == 0)
            {
                stockChance();
            }
            increaseLevel();
            SpawnerBall.healLives(1);
            SpawnRandomLevel();
            if (level % 5 == 0)
            {
                editChance();
            }
            SpawnerBall.Instance.RespawnBallFree();
        }
    }
    
    private void SpawnRandomLevel()
    {
        levelCleared = false;

        int[][] grid = GenerateRandomGrid();
        SpawnGrid(grid);
    }

    private int[][] GenerateRandomGrid()
    {
        int[][] grid = new int[rows][];

        for (int row = 0; row < rows; row++)
        {
            grid[row] = new int[cols];
            for (int col = 0; col < cols; col++)
            {
                int type = GetRandomCellType();
                if (type == -2)
                {
                    type = GetRandomCellType();
                }
                else
                {
                    grid[row][col] = type;
                }
            }
        }
        
        return grid;
    }

    private int GetRandomCellType()
    {
        int roll = Random.Range(0, 100);
        
        if (-1 < roll && roll <= chanceEmpty)
        {
            return EMPTY;
        }
        else if (chanceEmpty < roll && roll <= chanceEmpty+chanceSimple)
        {
            return SIMPLE;
        }
        else if (chanceEmpty+chanceSimple < roll &&  roll <= chanceEmpty+chanceSimple+chanceHard)
        {
            return HARD;
        }
        else if (chanceEmpty + chanceSimple + chanceHard < roll && roll <= chanceEmpty + chanceSimple + chanceHard + chanceGigaHard)
        {
            return GIGA_HARD;
        }
        else if (chanceEmpty + chanceSimple + chanceHard + chanceGigaHard < roll && roll <= chanceEmpty + chanceSimple + chanceHard + chanceGigaHard + chanceLucky)
        {
            return LUCKY;
        }
        else
        {
            return -2;
        }
    }

    private void SpawnGrid(int[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                int cellType = grid[row][col];
                
                if (cellType == EMPTY)
                {
                    continue;
                }
                Vector3 position = new Vector3(startX + col * blockWidth, startY - row * blockHeight, startZ);
                GameObject block;
                GameObject prefab = blockArray[cellType];
                block = Instantiate(prefab, position, Quaternion.identity);
                activeBlocks.Add(block);
            }
        }
    }

    private void stockChance()
    {
        chanceEmptyTemp =  chanceEmpty;
        chanceSimpleTemp =  chanceSimple;
        chanceHardTemp =  chanceHard;
        chanceGigaHardTemp = chanceGigaHard;
        chanceLuckyTemp =  chanceLucky;
    }
    private void increaseLevel()
    {
        
        if (level % 2 == 0)
        {
            if (level < 12)
            {
                chanceEmpty -= 5;
                chanceLucky -= 1;
                chanceSimple -= 2;
                chanceHard += 6;
                chanceGigaHard += 2;
            }
            else
            {
                if (chanceLucky > 1)
                {
                    chanceEmpty -= 2;
                    chanceLucky -= 1;
                    chanceSimple -= 2;
                    chanceHard += 4;
                    chanceGigaHard += 1;
                }
                else
                {
                    if (chanceSimple == 0 || chanceEmpty <= 4)
                    {
                        chanceHard -= 2;
                        chanceGigaHard += 2;
                        if (chanceHard <= 0)
                        {
                            chanceHard = 0;
                        }
                    }
                    else
                    {
                        chanceEmpty -= 2;
                        chanceSimple -= 2;
                        chanceHard += 3;
                        chanceGigaHard += 1;
                    }
                }
            }
        }
        else if  (level % 5 == 0)
        {
            int roll = Random.Range(0, 4);
            switch (roll)
            {
                case 0:
                    chanceEmpty = 100;
                    chanceSimple = 0;
                    chanceHard = 0;
                    chanceGigaHard = 0;
                    chanceLucky = 0;
                    break;
                case 1:
                    chanceEmpty = 0;
                    chanceSimple = 100;
                    chanceHard = 0;
                    chanceGigaHard = 0;
                    chanceLucky = 0;
                    break;
                case 2:
                    chanceEmpty = 0;
                    chanceSimple = 0;
                    chanceHard = 100;
                    chanceGigaHard = 0;
                    chanceLucky = 0;
                    break;
                case 3:
                    chanceEmpty = 0;
                    chanceSimple = 0;
                    chanceHard = 0;
                    chanceGigaHard = 100;
                    chanceLucky = 0;
                    break;
                case 4:
                    chanceEmpty = 0;
                    chanceSimple = 0;
                    chanceHard = 0;
                    chanceGigaHard = 0;
                    chanceLucky = 100;
                    break;
            }
        }
    }

    private void editChance()
    {
        chanceEmpty =  chanceEmptyTemp;
        chanceSimple =  chanceSimpleTemp;
        chanceHard =  chanceHardTemp;
        chanceGigaHard = chanceGigaHardTemp;
        chanceLucky =  chanceLuckyTemp;
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
        if (!isEstetique)
        {
            score += points * coefficient;
            scoreText.SetText($"{score} * {coefficient} points");
            if (audioSource != null && PointWingSong != null)
            {
                audioSource.PlayOneShot(PointWingSong);
            }
        }
    }
    
    /// <summary>Détruit tous les blocs restants, utile en cas de game over.</summary>
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

    public static void SetScore(int newscore)
    {
        score = newscore;
    }

    public static int GetLevel()
    {
        return level;
    }
}