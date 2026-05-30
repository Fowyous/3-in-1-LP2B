using UnityEngine;

public class PourrieScript : AppleScript
{
    private static float malusDuration = 5f;
    
    private static float seuil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 12f;
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
            speed = Mathf.Lerp(speed, 4f, Time.deltaTime * 0.5f);
    }
    
    protected override void OnCaught()
    {
        PanierScript.Instance.ApplyInvertedControls(malusDuration);
    }
}
