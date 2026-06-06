using TMPro;
using UnityEngine;

public class TotalScore : MonoBehaviour
{
    private int score;
    public TextMeshPro TextScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = Catchboy.scoreGet();
        TextScore.SetText("score : " + score);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
