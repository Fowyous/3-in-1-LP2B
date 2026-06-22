using UnityEngine;
using System.Collections.Generic;

///<summary>
///Displays the player's collected Bomb fragments in the HUD.
///Unlike a pre-filled bar, each icon only appears the instant a bomb
///fragment is actually collected (instant pop-in), creating a
///"charging up" visual effect as the player gathers fragments.
///All icons disappear at once when the special attack is used.
///
///SETUP IN UNITY:
///1. Create an empty GameObject in your HUD Canvas, e.g. "BombContainer".
///2. Add a Horizontal Layout Group to it (optional, for automatic spacing).
///3. Add this script to it.
///4. Assign "Bomb Icon Prefab" with a simple GameObject containing an Image
///   component using your bomb sprite.
///5. Assign "Player" with the UFO GameObject, or leave empty to auto-find it.
///</summary>
public class BombHUD : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Leave empty to auto-find the UFO in the scene.")]
    [SerializeField] private UFO player;

    [Tooltip("A simple prefab with an Image component showing your bomb sprite.")]
    [SerializeField] private GameObject bombIconPrefab;

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
                Debug.LogError("BombHUD: No UFO found in the scene!");
                return;
            }
        }

        if (iconsContainer == null) iconsContainer = transform;

        if (bombIconPrefab == null)
        {
            Debug.LogError("BombHUD: bombIconPrefab is not assigned!");
            return;
        }

        player.OnBombCountChanged += UpdateBombDisplay;
    }

    void OnDestroy()
    {
        if (player != null)
            player.OnBombCountChanged -= UpdateBombDisplay;
    }

    ///<summary>
    ///Called automatically whenever UFO.OnBombCountChanged fires.
    ///If the count went UP, instantly spawns one new icon (pop-in effect).
    ///If the count went back to 0 (special attack used), clears all icons at once.
    ///</summary>
    private void UpdateBombDisplay(int currentCount, int required)
    {
        if (currentCount == 0)
        {
            // Special attack was triggered: clear every icon at once
            ClearAllIcons();
            return;
        }

        // Spawn new icons until we match the current count
        // (covers the normal case of +1, and is safe if multiple are gained at once)
        while (spawnedIcons.Count < currentCount)
        {
            GameObject icon = Instantiate(bombIconPrefab, iconsContainer);
            spawnedIcons.Add(icon);
        }
    }

    ///<summary>
    ///Destroys every currently displayed bomb icon.
    ///</summary>
    private void ClearAllIcons()
    {
        foreach (var icon in spawnedIcons)
        {
            if (icon != null) Destroy(icon);
        }
        spawnedIcons.Clear();
    }
}