using UnityEngine;

public class PourrieScript : AppleScript
{
    private static float malusDuration = 5f;
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
        PanierScript.Instance.ApplyInvertedControls(malusDuration);
    }
}
