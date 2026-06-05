using UnityEngine;

public class AngelAppleScript : AppleScript
{
    private static int bonusLife = 1;
    
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
            speed = 8f;
    }
    
    protected override void OnCaught()
    {
        AppleSpawner.Instance.editHealth(bonusLife);
    }
    
}
