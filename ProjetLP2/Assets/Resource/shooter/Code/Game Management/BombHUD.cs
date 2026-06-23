using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
  [Tooltip("Sprite shown for a bomb slot that hasn't been collected yet.")]
  [SerializeField] private Sprite emptyBombIcon;
  [Tooltip("Sprite shown for a bomb slot that has been collected.")]
  [SerializeField] private Sprite filledBombIcon;

  private List<Image> bombIcons = new List<Image>();

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
    // Auto-find all Image components in children     
    bombIcons.AddRange(GetComponentsInChildren<Image>());
    if (bombIcons.Count == 0)
    {
      Debug.LogError("BombHUD: No Image components found in children!");
      return;
    }
    if (emptyBombIcon == null || filledBombIcon == null)
    {
      Debug.LogError("BombHUD: Empty and Filled bomb icons must be assigned!");
      return;
    }

    // Initialize all icons to empty state        
    RefreshAllIcons(0);

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
    RefreshAllIcons(currentCount);

  }

  ///<summary> 
  ///Updates all bomb icons based on the current count.
  ///Icons 0 to (count-1) show the filled sprite.    
  ///Icons from count onward show the empty sprite.    
  ///</summary>    
  private void RefreshAllIcons(int filledCount)
  {
    for (int i = 0; i < bombIcons.Count; i++)
    {
      if (i < filledCount)
      {
        // This slot has a bomb                
        bombIcons[i].sprite = filledBombIcon;
      }
      else
      {
        // This slot is empty                
        bombIcons[i].sprite = emptyBombIcon;
      }
    }
  }

}
