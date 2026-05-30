using UnityEngine;

public class TronionScript : AppleScript
{
    private static int malusScore = -1;
    private static float malusSpeed = 0.5f;
    private static float malusDuration = 3f;

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
        PanierScript.Instance.AddScore(malusScore);
        PanierScript.Instance.editSpeed(malusSpeed, malusDuration);
    }
}
