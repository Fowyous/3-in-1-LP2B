using UnityEngine;
using UnityEngine.SceneManagement;

public class AppleScript: MonoBehaviour
{
    public bool isEsthetique;

    void Start()
    {
        
    }
    
    void Update()
    {
        if (transform.position.y < -10f)
        {
            if (! isEsthetique)
            {
                AppleSpawner.Instance.LoseHealth();
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Destroy(gameObject);
    }
    
    public void setIsEsthetique(bool isEsthetique)
    {
        this.isEsthetique = isEsthetique;
    }
}