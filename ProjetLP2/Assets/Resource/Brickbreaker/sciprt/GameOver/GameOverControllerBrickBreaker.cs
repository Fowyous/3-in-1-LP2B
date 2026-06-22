using TMPro;
using UnityEngine;

/// <summary>
/// Displays the final score and run statistics (bricks destroyed by type)
/// on the Game Over screen, then resets the score for the next playthrough.
/// </summary>
public class GameOverControllerBrickBreaker : MonoBehaviour
{
    public TextMeshPro scoreText;
    public TextMeshPro statisticsText;

    private int score;

    private static int simpleBricksDestroyed = 0;
    private static int hardBricksDestroyed   = 0;
    private static int gigaHardBrickDestroy  = 0;
    private static int luckyBricksDestroyed  = 0;

    private void Start()
    {
        score = BrickSpawner.GetScore();
        scoreText.SetText("score : " + score);
        BrickSpawner.SetScore(0);
    }

    private void Update()
    {
        statisticsText.SetText("Level : " + BrickSpawner.GetLevel() + "\n" +
                               "Simple brick destroy: " + simpleBricksDestroyed + "\n" +
                               "Hard brick destroy: " + hardBricksDestroyed + "\n" +
                               "Giga Hard brick destroy: " + gigaHardBrickDestroy + "\n" +
                               "Lucky brick destroy: " + luckyBricksDestroyed);
    }

    public static void editNumberSimple(int number)
    {
        simpleBricksDestroyed += number;
    }

    public static void editNumberHard(int number)
    {
        hardBricksDestroyed += number;
    }

    public static void editNumberGigaHard(int number)
    {
        gigaHardBrickDestroy += number;
    }
    
    public static void editNumberLucky(int number)
    {
        luckyBricksDestroyed += number;
    }
}