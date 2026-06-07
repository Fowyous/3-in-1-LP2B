using UnityEngine;

public class PourrieScript : AppleScript
{
    private static float malusDuration;
    private static float speed;
    private static int damage;
    private static int numberCatchPourrie;

    private static float seuil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 12f;
        seuilAttaque = 0f;
        coefficient = -3;
        malusDuration = 5f;
        damage = 0;
        numberCatchPourrie = 0;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
        
        if (transform.position.y < -seuil)
        {
            if (!isEsthetique)
            {
                OnMissed();
            }
            Destroy(gameObject);
        }
    }
    
    protected override void UpdateSpeed()
    {
        if (transform.position.y < seuilAttaque)
            speed = 4f;
    }
    
    protected override void OnCaught()
    {
        Statistiques.editNumberCatchPourrie(1);
        Catchboy.Instance.editCoefficient(coefficient);
        Catchboy.Instance.ApplyInvertedControls(malusDuration);
    }
    
    protected override void OnMissed()
    {
        Catchboy.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }
}
