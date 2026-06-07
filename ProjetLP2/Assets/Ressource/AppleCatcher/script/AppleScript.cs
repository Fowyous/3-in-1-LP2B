using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public bool isEsthetique;
    private int damage;
    private int score;
    private float seuil;
    public int coefficient;
    protected static float speed;
    protected float seuilAttaque;
    protected static int numberCatch;

    protected virtual void Start()
    {
        speed = 8f;
        seuilAttaque = 0f;
        coefficient = 1;
        seuil = 6f;
        score = 1;
        damage = -1;
        numberCatch = 0;
    }

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

    protected virtual void UpdateSpeed() { }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("catchboy"))
        {
            OnCaught();
            Destroy(gameObject);
        }
    }

    protected virtual void OnCaught()
    {
        Statistiques.editNumberApple(1);
        Catchboy.Instance.editCoefficient(coefficient);
        Catchboy.Instance.AddScore(score);
    }

    protected virtual void OnMissed()
    {
        Catchboy.Instance.editCoefficient(0);
        AppleSpawner.Instance.editHealth(damage);
    }

    public void setIsEsthetique(bool value)
    {
        isEsthetique = value;
    }

    public float getSeuil()
    {
        return seuil;
    }
}