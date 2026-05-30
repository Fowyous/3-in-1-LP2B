using Unity.VisualScripting;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public bool isEsthetique;
    private static int damage = -1;
    private static int score = 1;
    private static float seuil = 10f;

    void Update()
    {
        if (transform.position.y < -seuil)
        {
            if (!isEsthetique)
            {
                OnMissed();
            }
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("panier"))
        {
            OnCaught();
            Destroy(gameObject);
        }
    }

    protected virtual void OnCaught()
    {
        PanierScript.Instance.AddScore(score);
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