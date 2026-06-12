using UnityEngine;

public class GoldenAppleScript : AppleScript
{
    private static int bonusScore;
    private static float bonusSpeed;
    private static float bonusDuration;
    private static float speed;
    private static int damage;
    private static float seuil;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        seuil = getSeuil();
        speed = 4f;
        seuilAttaque = 0f;
        coefficient = 3;
        bonusScore = 2;
        bonusSpeed = 1.5f;
        bonusDuration = 20f;
        damage = 0;
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
        {
            speed = 8f;
        }
    }
    
    protected override void OnCaught()
    {
        Statistiques.editNumberCatchGolden(1);
        Catchboy.Instance.editCoefficient(coefficient);
        Catchboy.Instance.AddScore(bonusScore);
        Catchboy.Instance.editSpeed(bonusSpeed,  bonusDuration);
    }
    
    protected override void OnMissed()
    {
        Catchboy.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }
}
