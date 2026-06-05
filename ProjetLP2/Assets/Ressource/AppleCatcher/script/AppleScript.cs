using Unity.VisualScripting;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public bool isEsthetique;
    private static int damage = -1;
    private static int score = 1;
    private static float seuil = 6f;
    protected float speed;
    protected float seuilAttaque;

    protected virtual void Start()
    {
        speed = 8f;
        seuilAttaque = 0f;
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
        Catchboy.Instance.AddScore(score);
    }

    protected virtual void OnMissed()
    {
        AppleSpawner.Instance.editHealth(damage);
    }

    public void setIsEsthetique(bool value)
    {
        isEsthetique = value;
    }

    public static float getSeuil()
    {
        return seuil;
    }
}