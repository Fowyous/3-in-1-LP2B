using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using static ShooterConstants;

///<summary>
///spawns asteroids.
///</summary> 
public class AsteroidSpawner : MonoBehaviour
{
  [Header("Spawn Settings")]
  [SerializeField] private List<GameObject> Asteroids = new List<GameObject>();
  [SerializeField] private float spawnInterval = 3f; // Delay in seconds between each spawn

  private string requiredScriptName = "Asteroid"; // All objects has to have the asteroids objects
  private void OnValidate()
  {
    for (int i = Asteroids.Count - 1; i >= 0; i--)
    {
      if (Asteroids[i] == null)
        continue;

      if (Asteroids[i].GetComponent(requiredScriptName) == null)
      {
        Debug.LogWarning($"{Asteroids[i].name} does not have {requiredScriptName} Script!", Asteroids[i]);
        Asteroids.RemoveAt(i);
      }
    }
  }


  ///<summary>
  ///Infinite coroutine that spawns a random asteriod every spawnInterval seconds.
  ///</summary>
  private IEnumerator SpawnLoop()
  {
    while (true)
    {
      SpawnRandom();
      yield return new WaitForSeconds(spawnInterval);
    }
  }


  private void SpawnRandom()
  {
    // Spawn at the right edge of the screen, at a random Y position within game bounds
    Vector3 spawnPosition = new Vector3(
        ShooterConstants.GameLimit.x + 2f,
        Random.Range(-ShooterConstants.GameLimit.y, ShooterConstants.GameLimit.y),
        0f
    );

    GameObject newAsteroid = Instantiate(Asteroids[Random.Range(0, Asteroids.Count)], spawnPosition, Quaternion.identity);


  }
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    StartCoroutine(SpawnLoop());

  }

  // Update is called once per frame
  void Update()
  {

  }
}
