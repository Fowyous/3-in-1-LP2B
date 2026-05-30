using UnityEngine;

public class GoldenAppleScript : AppleScript
{
    private static int bonusScore = 2;
    private static float bonusSpeed = 1.5f;
    private static float bonusDuration = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void OnCaught()
    {
        PanierScript.Instance.AddScore(bonusScore);
        PanierScript.Instance.editSpeed(bonusSpeed,  bonusDuration);
    }
}
