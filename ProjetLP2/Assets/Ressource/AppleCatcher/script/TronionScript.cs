using UnityEngine;

public class TronionScript : AppleScript
{
    private static int malusScore;
    private static float malusSpeed;
    private static float malusDuration;
    private static float speed;
    private static int damage;
    private static int numberCatchTronion;

    private static float seuil;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 12f;
        seuilAttaque = 0f;
        coefficient = -2;
        malusScore = -1;
        malusSpeed = 0.5f;
        malusDuration = 3f;
        damage = 0;
        numberCatchTronion = 0;
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
        Statistiques.editNumberCatchTronion(1);
        Catchboy.Instance.editCoefficient(coefficient);
        Catchboy.Instance.AddScore(malusScore);
        Catchboy.Instance.editSpeed(malusSpeed, malusDuration);
    }
    
    protected override void OnMissed()
    {
        Catchboy.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }
}
