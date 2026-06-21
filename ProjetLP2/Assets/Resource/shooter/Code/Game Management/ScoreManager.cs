using UnityEngine;
using TMPro;
using System;

///<summary>
///Singleton that tracks the player's score across the whole game session.
///Score increases when an enemy dies, based on its max health value
///(e.g. a Boss with 15 HP grants more points than a Kamikaz with 1 HP).
///</summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI References (TextMeshPro)")]
    [Tooltip("TextMeshPro element shown in the HUD during gameplay.")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Tooltip("TextMeshPro element shown on the Game Over screen.")]
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    public int CurrentScore { get; private set; } = 0;

    ///<summary>
    ///Fired whenever the score changes. Can be used by other UI scripts if needed.
    ///</summary>
    public event Action<int> OnScoreChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple ScoreManager instances detected!");
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateScoreDisplay();
    }

    ///<summary>
    ///Adds points to the score. Called by enemies when they die, passing their max health.
    ///</summary>
    public void AddScore(float points)
    {
        CurrentScore += Mathf.RoundToInt(points);
        UpdateScoreDisplay();
        OnScoreChanged?.Invoke(CurrentScore);
        Debug.Log($"Score updated: +{points} -> Total: {CurrentScore}");
    }

    ///<summary>
    ///Updates both the in-game HUD score text and (if assigned) the Game Over score text.
    ///</summary>
    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {CurrentScore}";

        if (gameOverScoreText != null)
            gameOverScoreText.text = $"Final Score: {CurrentScore}";
    }

    ///<summary>
    ///Resets the score to zero. Call this when restarting the game.
    ///</summary>
    public void ResetScore()
    {
        CurrentScore = 0;
        UpdateScoreDisplay();
    }
}