using UnityEngine;
using System.Collections.Generic;

///<summary>
///Manages the row of icons showing which timed power-ups (RapidFire, SpeedBoost,
///Shield) are currently active on the UFO, each with its own radial countdown.
///An icon appears the instant a power-up activates and disappears the instant
///it ends (or its countdown finishes, whichever comes first).
///

///</summary>
public class ActivePowerUpsHUD : MonoBehaviour
{
    [System.Serializable]
    public class IconMapping
    {
        public PowerUpType type;
        public GameObject iconPrefab;
    }

    [Header("References")]
    [Tooltip("Leave empty to auto-find the UFO in the scene.")]
    [SerializeField] private UFO player;

    [Tooltip("One icon prefab per timed power-up type (RapidFire, SpeedBoost, Shield).")]
    [SerializeField] private List<IconMapping> iconPrefabs = new List<IconMapping>();

    [Tooltip("Parent container where icons are instantiated. Defaults to this GameObject's transform.")]
    [SerializeField] private Transform iconsContainer;

    // Tracks the currently displayed icon instance for each active power-up type
    private Dictionary<PowerUpType, GameObject> activeIcons = new Dictionary<PowerUpType, GameObject>();

    void Start()
    {
        if (player == null)
        {
            player = Object.FindAnyObjectByType<UFO>();
            if (player == null)
            {
                Debug.LogError("ActivePowerUpsHUD: No UFO found in the scene!");
                return;
            }
        }

        if (iconsContainer == null) iconsContainer = transform;

        player.OnPowerUpActivated += HandlePowerUpActivated;
        player.OnPowerUpDeactivated += HandlePowerUpDeactivated;
    }

    void OnDestroy()
    {
        if (player != null)
        {
            player.OnPowerUpActivated -= HandlePowerUpActivated;
            player.OnPowerUpDeactivated -= HandlePowerUpDeactivated;
        }
    }

    ///<summary>
    ///Spawns (or restarts) the icon for the given power-up type with its countdown.
    ///</summary>
    private void HandlePowerUpActivated(PowerUpType type, float duration)
    {
        // If already shown (shouldn't normally happen since effects don't stack), restart its timer
        if (activeIcons.TryGetValue(type, out GameObject existingIcon) && existingIcon != null)
        {
            ActivePowerUpIcon iconScript = existingIcon.GetComponent<ActivePowerUpIcon>();
            if (iconScript != null) iconScript.StartCountdown(duration);
            return;
        }

        GameObject prefab = GetPrefabForType(type);
        if (prefab == null)
        {
            Debug.LogWarning($"ActivePowerUpsHUD: No icon prefab configured for {type}.");
            return;
        }

        GameObject newIcon = Instantiate(prefab, iconsContainer);
        activeIcons[type] = newIcon;

        ActivePowerUpIcon newIconScript = newIcon.GetComponent<ActivePowerUpIcon>();
        if (newIconScript != null) newIconScript.StartCountdown(duration);
    }

    ///<summary>
    ///Removes the icon for the given power-up type, if currently shown.
    ///</summary>
    private void HandlePowerUpDeactivated(PowerUpType type)
    {
        if (activeIcons.TryGetValue(type, out GameObject icon))
        {
            if (icon != null) Destroy(icon);
            activeIcons.Remove(type);
        }
    }

    private GameObject GetPrefabForType(PowerUpType type)
    {
        foreach (var mapping in iconPrefabs)
        {
            if (mapping.type == type) return mapping.iconPrefab;
        }
        return null;
    }
}