using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockSpawner  : MonoBehaviour 
{
    public static BlockSpawner Instance { get; private set; }

    private const int EMPTY  = -1;
    private const int SIMPLE = 0;
    private const int HARD   = 1;
    private const int Lucky  = 2;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] blockArray;
    [SerializeField]  private TextMeshPro scoreText;
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

    [Header("Probabilités sur 100")]
    [Range(0, 100)] [SerializeField] private int chanceEmpty;
    [Range(0, 100)] [SerializeField] private int chanceSimple;
    [Range(0, 100)] [SerializeField] private int chanceHard;
    [Range(0, 100)] [SerializeField] private int chanceLucky;


    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool levelCleared = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        int score = 0;
        SpawnRandomLevel();
        scoreText.SetText(score + "points");
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
            /*il faut mettre une fonction pour que la balle réaparaisse devant le paddel*/
            SpawnerBall.healLives(1);
            SpawnRandomLevel();
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
        
        if (0 <= roll && roll <= chanceEmpty)
        {
            return EMPTY;
        }
        else if (chanceEmpty <= roll && roll <= chanceEmpty+chanceSimple)
        {
            return SIMPLE;
        }
        else if (chanceEmpty+chanceSimple <= roll &&  roll <= chanceEmpty+chanceSimple+chanceHard)
        {
            return HARD;
        }
        else if (chanceEmpty + chanceSimple + chanceHard <= roll && roll <= chanceEmpty + chanceSimple + chanceHard + chanceLucky)
        {
            return Lucky;
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
                Vector3 position = new Vector3(startX + col * blockWidth, startY - row * blockHeight, 0f);
                GameObject block;
                GameObject prefab = blockArray[cellType];
                block = Instantiate(prefab, position, Quaternion.identity);
                activeBlocks.Add(block);
            }
        }
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
        score += points * coefficient;
        scoreText.SetText($"{score} * {coefficient} points");
        if (audioSource != null && PointWingSong != null)
            audioSource.PlayOneShot(PointWingSong);
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
}