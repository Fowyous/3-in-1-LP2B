using UnityEngine;

public class AngelAppleScript : AppleScript
{
    private static int bonusLife = 1;
    
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
        AppleSpawner.Instance.editHealth(bonusLife);
    }
    
}
