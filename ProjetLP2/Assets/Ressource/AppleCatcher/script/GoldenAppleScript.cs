using UnityEngine;

public class GoldenAppleScript : AppleScript
{
    private static int bonusScore = 2;
    private static float bonusSpeed = 1.5f;
    private static float bonusDuration = 20f;
    
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
        PanierScript.Instance.AddScore(bonusScore);
        PanierScript.Instance.editSpeed(bonusSpeed,  bonusDuration);
    }
}
