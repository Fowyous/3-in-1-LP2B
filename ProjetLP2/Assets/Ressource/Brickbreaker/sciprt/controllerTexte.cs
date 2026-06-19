using TMPro;
using UnityEngine;

public class controllerTexte : MonoBehaviour
{
    private int score;
    public TextMeshPro TextScore;
    
    public TextMeshPro TextStatistique;

    private static int numberSimple = 0;
    private static int numberHard = 0;
    private static int numberLucky = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = BlockSpawner.GetScore();
        TextScore.SetText("score : " + score);
        BlockSpawner.SetScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        TextStatistique.SetText("Simple block destroy: " + numberSimple + "\n" +
                                "Hard block destroy: " + numberHard + "\n" +
                                "Lucky block destroy: " + numberLucky);
    }
    
    public static void editNumberSimple(int number)
    {
        numberSimple += number;
    }

    public static void editNumberHard(int number)
    {
        numberHard += number;
    }

    public static void editNumberLucky(int number)
    {
        numberLucky += number;
    }
}
