using UnityEngine;
using System.Collections.Generic;
using static ShooterConstants;

public class EnemySpawner : MonoBehaviour
{
  //this is the list of enemies that will be added through the unity editor
  [SerializeField] private List<GameObject> enemiesPrefabs = new List<GameObject>();
  public GameObject player;

  /// <summary>
  /// Loads a random enemy.
  /// </summary>
  private void LoadRandomEnemy()
  {

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
