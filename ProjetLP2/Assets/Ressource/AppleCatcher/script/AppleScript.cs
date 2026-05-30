using UnityEngine;
using UnityEngine.SceneManagement;

public class AppleScript : MonoBehaviour
{
    void Update()
    {
        if (transform.position.y < -10f)
        {
            if (SceneManager.GetActiveScene().name == "AppleCatcherGame")
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
}