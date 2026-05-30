using UnityEngine;

public class TronionScript : AppleScript
{
    private static int malusScore = -1;
    private static float malusSpeed = 0.5f;
    private static float malusDuration = 3f;
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
        PanierScript.Instance.AddScore(malusScore);
        PanierScript.Instance.editSpeed(malusSpeed, malusDuration);
    }
}
