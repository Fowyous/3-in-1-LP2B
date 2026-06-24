using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

///<summary>
///Displays the UFO's health as a row of life icons (e.g. hearts/sprites).
///Automatically generates one icon per max health point at Start,
///then hides icons one by one as the UFO loses health.
///</summary>
public class UFOHealthUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Leave empty to auto-find the UFO in the scene.")]
    [SerializeField] private UFO player;

    [Tooltip("A simple prefab with an Image component showing your life sprite.")]
    [SerializeField] private GameObject lifeIconPrefab;

    [Tooltip("Parent container where icons are instantiated. Defaults to this GameObject's transform.")]
    [SerializeField] private Transform iconsContainer;

    private List<GameObject> spawnedIcons = new List<GameObject>();

    void Start()
    {
        if (player == null)
        {
            player = Object.FindAnyObjectByType<UFO>();
            if (player == null)
            {
                Debug.LogError("UFOHealthUI: No UFO found in the scene!");
                return;
            }
        }

        if (iconsContainer == null) iconsContainer = transform;

        if (lifeIconPrefab == null)
        {
            Debug.LogError("UFOHealthUI: lifeIconPrefab is not assigned!");
            return;
        }

        // Subscribe to health changes
        player.OnHealthChanged += UpdateHealthDisplay;

        // Generate icons matching the UFO's max health
        GenerateIcons((int)player.MaxHealth);

        // Initial display matching current health
        UpdateHealthDisplay(player.CurrentHealth, player.MaxHealth);
    }

    void OnDestroy()
    {
        if (player != null)
            player.OnHealthChanged -= UpdateHealthDisplay;
    }

    ///<summary>
    ///Instantiates one life icon per max health point, inside the container.
    ///</summary>
    private void GenerateIcons(int count)
    {
        // Clear any pre-existing icons (in case of re-generation)
        foreach (var icon in spawnedIcons)
        {
            if (icon != null) Destroy(icon);
        }
        spawnedIcons.Clear();

        for (int i = 0; i < count; i++)
        {
            GameObject icon = Instantiate(lifeIconPrefab, iconsContainer);
            spawnedIcons.Add(icon);
        }
    }

    ///<summary>
    ///Shows exactly as many icons as the current health value (rounded up),
    ///hiding the rest. Called automatically whenever UFO.OnHealthChanged fires.
    ///</summary>
    private void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        int visibleCount = Mathf.CeilToInt(currentHealth);

        for (int i = 0; i < spawnedIcons.Count; i++)
        {
            if (spawnedIcons[i] != null)
                spawnedIcons[i].SetActive(i < visibleCount);
        }
    }
}