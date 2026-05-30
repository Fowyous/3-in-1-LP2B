using UnityEngine;

public class AngelAppleScript : AppleScript
{
    private static int bonusLife = 1;
    
    private static float seuil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seuil = getSeuil();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -seuil)
        {
            Destroy(gameObject);
        }
    }
    
    protected override void OnCaught()
    {
        AppleSpawner.Instance.editHealth(bonusLife);
    }
    
}
