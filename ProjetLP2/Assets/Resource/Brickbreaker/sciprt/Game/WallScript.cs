using UnityEngine;

/// <summary>
/// Plays a collision sound whenever the ball (or anything else) hits this wall.
/// </summary>
public class WallScript : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip wallSong;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (audioSource != null && wallSong != null)
            audioSource.PlayOneShot(wallSong);
    }
}