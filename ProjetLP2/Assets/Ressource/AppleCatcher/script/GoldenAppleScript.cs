using UnityEngine;

public class GoldenAppleScript : AppleScript
{
    private static int bonusScore = 2;
    private static float bonusSpeed = 1.5f;
    private static float bonusDuration = 20f;
    
    private static float seuil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 4f;
        seuilAttaque = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
        
        if (transform.position.y < -seuil)
        {
            Destroy(gameObject);
        }
    }
    
    protected override void UpdateSpeed()
    {
        if (transform.position.y < seuilAttaque)
        {
            speed = 8f;
        }
    }
    
    protected override void OnCaught()
    {
        PanierScript.Instance.AddScore(bonusScore);
        PanierScript.Instance.editSpeed(bonusSpeed,  bonusDuration);
    }
}
