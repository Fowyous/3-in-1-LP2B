using TMPro;
using UnityEngine;

/// <summary>
/// Displays the final score and run statistics (apple dcatched by type)
/// on the Game Over screen, then resets the score for the next playthrough.
/// </summary>
public class GameOverControllerAppleCatcher : MonoBehaviour
{
    public TextMeshPro statisticsText;

    private static int numberApple = 0;
    private static int numberCatchGolden = 0;
    private static int numberCatchPourrie = 0;
    private static int numberCatchTronion = 0;
    private static int numberCatchAngel = 0;
    
    private int score;
    public TextMeshPro TextScore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = CatchboyController.scoreGet();
        TextScore.SetText("score : " + score);
    }

    // Update is called once per frame
    void Update()
    {
        statisticsText.SetText("Apple catch : " + numberApple + "\n" + 
                               "Angel Apple Catch: " + numberCatchAngel + "\n" +
                               "Golden Apple Catch: " + numberCatchGolden + "\n" +
                               "Pourri Apple Catch: " + numberCatchPourrie + "\n" +
                               "Tronion Apple Catch: " + numberCatchTronion + "\n");
    }
    
    public static void editNumberApple(int number)
    {
        numberApple += number;
    }

    public static void editNumberCatchGolden(int number)
    {
        numberCatchGolden += number;
    }

    public static void editNumberCatchPourrie(int number)
    {
        numberCatchPourrie += number;
    }

    public static void editNumberCatchTronion(int number)
    {
        numberCatchTronion += number;
    }

    public static void editNumberCatchAngel(int number)
    {
        numberCatchAngel += number;
    }
}
