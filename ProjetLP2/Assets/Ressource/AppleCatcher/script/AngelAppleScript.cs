using UnityEngine;

public class AngelAppleScript : AppleScript
{
    private static int bonusLife;
    private static float speed;
    private static float seuil;
    private static int damage;
    private static int numberCatchAngel;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 4f;
        seuilAttaque = 0f;
        coefficient = 5;
        damage = 0;
        bonusLife = 1;
        numberCatchAngel = 0;
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
            speed = 8f;
    }
    
    protected override void OnCaught()
    {
        Statistiques.editNumberCatchAngel(1);
        Catchboy.Instance.editCoefficient(coefficient);
        AppleSpawner.Instance.editHealth(bonusLife);
    }
    
    protected override void OnMissed()
    {
        Catchboy.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }
}