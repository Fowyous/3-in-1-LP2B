using UnityEngine;
using System.Collections.Generic;
using static ShooterConstants;

public class EnemySpawner : MonoBehaviour
{
    private void OnValidate()
    {
        for (int i = enemiesPrefabs.Count - 1; i >= 0; i--)
        {
            if (enemiesPrefabs[i] != null && enemiesPrefabs[i].GetComponent<IEnemy>() == null)
            {
                Debug.LogWarning($"'{enemiesPrefabs[i].name}' doesn't implement IEnemy. Removed from list.");
                enemiesPrefabs.RemoveAt(i);
            }
        }
    }

    // This is the list of enemies that will be added through the unity editor
    [SerializeField] private List<GameObject> enemiesPrefabs = new List<GameObject>();
    public GameObject player;

    /// <summary>
    /// Loads a random enemy and automatically finds the target player if missing.
    /// </summary>
    private void LoadRandomEnemy()
    {
        // AUTOMATIC FIX: If the player target is missing, find the UFO in the scene dynamically
        if (player == null)
        {
            // Updated to modern Unity 6 standards to prevent CS0618 deprecation warning
            UFO foundPlayer = Object.FindAnyObjectByType<UFO>();
            if (foundPlayer != null)
            {
                player = foundPlayer.gameObject;
            }
            else
            {
                Debug.LogError("EnemySpawner: Player Target (UFO) is missing from the scene!");
                return; // Block spawning to prevent enemy AI crashes
            }
        }

        GameObject newenemy = Instantiate(enemiesPrefabs[Random.Range(0, enemiesPrefabs.Count)]);

        var enemyScript = newenemy.GetComponent<IEnemy>();
        if (enemyScript != null) enemyScript.Target = player;
        newenemy.transform.position = new Vector3(10.5f, Random.Range(-ShooterConstants.GameLimit.y, ShooterConstants.GameLimit.y));
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadRandomEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}