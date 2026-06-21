using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static ShooterConstants;

///<summary>
///Spawns decorative asteroids with a parallax effect:
///asteroids are randomly assigned a "depth layer" (far / mid / near),
///each layer having its own speed, scale, and sorting order so closer
///asteroids move faster and appear bigger, creating an illusion of depth.
///All asteroids are forced onto a dedicated "Background" sorting layer
///so they always render behind enemies, the UFO, and the base.
///</summary>
public class AsteroidSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public string layerName = "Far";
        [Tooltip("Movement speed for this layer.")]
        public float speed = 2f;
        [Tooltip("Scale multiplier applied to the asteroid sprite.")]
        public float scale = 0.5f;
        [Tooltip("Sorting order WITHIN the Background sorting layer (higher = drawn closer to camera, but still behind gameplay layer).")]
        public int sortingOrder = 0;
        [Tooltip("Relative chance of spawning an asteroid on this layer.")]
        public float weight = 1f;
    }

    [Header("Spawn Settings")]
    [SerializeField] private List<GameObject> Asteroids = new List<GameObject>();
    [SerializeField] private float spawnInterval = 2f;
    private string requiredScriptName = "Asteroid";

    [Header("Rendering")]
    [Tooltip("Name of the Sorting Layer used to keep asteroids behind all gameplay elements. Create it in Edit > Project Settings > Tags and Layers, and place it ABOVE 'Default' in the list.")]
    [SerializeField] private string backgroundSortingLayer = "Background";

    [Header("Parallax Layers")]
    [SerializeField] private List<ParallaxLayer> layers = new List<ParallaxLayer>
    {
        new ParallaxLayer { layerName = "Far",  speed = 1.5f, scale = 0.4f, sortingOrder = 0, weight = 3f },
        new ParallaxLayer { layerName = "Mid",  speed = 3f,   scale = 0.7f, sortingOrder = 1, weight = 2f },
        new ParallaxLayer { layerName = "Near", speed = 6f,   scale = 1.2f, sortingOrder = 2, weight = 1f },
    };

    private void OnValidate()
    {
        for (int i = Asteroids.Count - 1; i >= 0; i--)
        {
            if (Asteroids[i] == null) continue;
            if (Asteroids[i].GetComponent(requiredScriptName) == null)
            {
                Debug.LogWarning($"{Asteroids[i].name} does not have {requiredScriptName} Script!", Asteroids[i]);
                Asteroids.RemoveAt(i);
            }
        }
    }

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
        if (Asteroids.Count == 0) return;

        Vector3 spawnPosition = new Vector3(
            ShooterConstants.GameLimit.x + 2f,
            Random.Range(-ShooterConstants.GameLimit.yBottom, ShooterConstants.GameLimit.yTop),
            0f
        );

        GameObject newAsteroid = Instantiate(
            Asteroids[Random.Range(0, Asteroids.Count)],
            spawnPosition,
            Quaternion.identity
        );

        ParallaxLayer chosenLayer = GetWeightedRandomLayer();
        if (chosenLayer == null) return;

        Asteroid asteroidScript = newAsteroid.GetComponent<Asteroid>();
        if (asteroidScript != null)
        {
            asteroidScript.SetParallaxValues(chosenLayer.speed, chosenLayer.scale);
        }

        // Force the asteroid onto the dedicated Background sorting layer,
        // so it always renders behind enemies, the UFO, and the base.
        SpriteRenderer sr = newAsteroid.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingLayerName = backgroundSortingLayer;
            sr.sortingOrder = chosenLayer.sortingOrder;
        }
        else
        {
            Debug.LogWarning($"AsteroidSpawner: '{newAsteroid.name}' has no SpriteRenderer, cannot apply background sorting.");
        }
    }

    private ParallaxLayer GetWeightedRandomLayer()
    {
        float totalWeight = 0f;
        foreach (var layer in layers) totalWeight += layer.weight;

        if (totalWeight <= 0f) return layers.Count > 0 ? layers[0] : null;

        float randomValue = Random.Range(0f, totalWeight);
        foreach (var layer in layers)
        {
            randomValue -= layer.weight;
            if (randomValue <= 0f) return layer;
        }

        return layers[layers.Count - 1];
    }

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }
}