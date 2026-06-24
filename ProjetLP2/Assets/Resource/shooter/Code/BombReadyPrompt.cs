using UnityEngine;
using TMPro;

///<summary>
///Shows a "Press [T] to use your bomb" message in the HUD whenever the player
///has collected enough Bomb fragments to trigger the special attack, and
///hides it otherwise.
///
///SETUP IN UNITY:
///1. Create a TextMeshPro - Text (UI) element in your HUD Canvas, e.g. "BombReadyText".
///   Set its text to something like "Press [T] to use your bomb!".
///2. Create an empty GameObject (or reuse the text's GameObject) and add this script to it.
///3. Assign "Prompt Text" with that TextMeshPro element.
///4. Assign "Player" with the UFO GameObject, or leave empty to auto-find it.
///</summary>
public class BombReadyPrompt : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Leave empty to auto-find the UFO in the scene.")]
    [SerializeField] private UFO player;

    [Tooltip("The TextMeshPro element showing the 'Press [T]' message.")]
    [SerializeField] private TextMeshProUGUI promptText;

    void Start()
    {
        if (player == null)
        {
            player = Object.FindAnyObjectByType<UFO>();
            if (player == null)
            {
                Debug.LogError("BombReadyPrompt: No UFO found in the scene!");
                return;
            }
        }

        if (promptText == null)
        {
            Debug.LogError("BombReadyPrompt: promptText is not assigned!");
            return;
        }

        player.OnBombCountChanged += UpdatePrompt;

        // Start hidden until we know the current state
        promptText.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (player != null)
            player.OnBombCountChanged -= UpdatePrompt;
    }

    ///<summary>
    ///Shows the prompt only when the player has enough bombs to trigger the special attack.
    ///Called automatically whenever UFO.OnBombCountChanged fires.
    ///</summary>
    private void UpdatePrompt(int currentCount, int required)
    {
        bool isReady = currentCount >= required;
        promptText.gameObject.SetActive(isReady);
    }
}
