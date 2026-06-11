using UnityEngine;

public class WallScript : MonoBehaviour
{
    [Header("Song")]
    [SerializeField] public AudioClip wallSong;
    private static AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (audioSource != null && wallSong != null)
            audioSource.PlayOneShot(wallSong);
    }
}
