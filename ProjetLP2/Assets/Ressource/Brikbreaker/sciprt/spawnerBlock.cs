using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    private const int EMPTY  = 0;
    private const int SIMPLE = 1;
    private const int HARD   = 2;

    [Header("Prefabs")]
    [SerializeField] private GameObject simpleBlockPrefab;
    [SerializeField] private GameObject hardBlockPrefab;
    [SerializeField]  private TextMeshPro scoreText;
    private int score = 0;

    [Header("Taille de la grille")]
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 7;

    [Header("Espacement des blocs")]
    [SerializeField] private float blockWidth  = 1.76f;
    [SerializeField] private float blockHeight = 0.96f;
    [SerializeField] private float startX      = -8.5f;
    [SerializeField] private float startY      =  4.5f;

    [Header("Probabilités sur 100")]
    [Range(0, 100)] [SerializeField] private int chanceEmpty  = 20;
    [Range(0, 100)] [SerializeField] private int chanceSimple = 50;

    private List<GameObject> activeBlocks = new List<GameObject>();
    private bool levelCleared = false;

    void Start()
    {
        SpawnRandomLevel();
        scoreText.SetText(score + "points");
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
        int[][] grid     = new int[rows][];
        bool    hasBlock = false; 

        for (int row = 0; row < rows; row++)
        {
            grid[row] = new int[cols];
            for (int col = 0; col < cols; col++)
            {
                grid[row][col] = GetRandomCellType();
                if (grid[row][col] != EMPTY) hasBlock = true;
            }
        }

        if (!hasBlock)
        {
            grid[rows / 2][cols / 2] = SIMPLE;
        }

        return grid;
    }

    private int GetRandomCellType()
    {
        int roll = Random.Range(0, 100);

        if (roll < chanceEmpty)                    return EMPTY;
        if (roll < chanceEmpty + chanceSimple)     return SIMPLE;
        return HARD;
    }

    private void SpawnGrid(int[][] grid)
    {
        for (int row = 0; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[row].Length; col++)
            {
                int cellType = grid[row][col];
                if (cellType == EMPTY) continue;

                Vector3 position = new Vector3(
                    startX + col * blockWidth,
                    startY - row * blockHeight,
                    0f
                );

                GameObject prefab = (cellType == SIMPLE) ? simpleBlockPrefab : hardBlockPrefab;
                GameObject block  = Instantiate(prefab, position, Quaternion.identity);
                activeBlocks.Add(block);
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
}