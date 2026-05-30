using UnityEngine;

public class PourrieScript : AppleScript
{
    private static float malusDuration = 5f;
    
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
        PanierScript.Instance.ApplyInvertedControls(malusDuration);
    }
}
