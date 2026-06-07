using TMPro;
using UnityEngine;

public class Statistiques : MonoBehaviour
{
    public TextMeshPro TextStatistique;

    private static int numberApple = 0;
    private static int numberCatchGolden = 0;
    private static int numberCatchPourrie = 0;
    private static int numberCatchTronion = 0;
    private static int numberCatchAngel = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TextStatistique.SetText("Apple catch : " + numberApple + "\n" + 
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
