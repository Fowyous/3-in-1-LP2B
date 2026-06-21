using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public Slider timeSlider; 
    public bool isPlaying = false;
    void Start()
    {
        if (videoPlayer != null && timeSlider != null)
        {
            timeSlider.maxValue = (float)videoPlayer.length;
            videoPlayer.loopPointReached += OnVideoEnded;
        }
    }

    void Update()
    {
        // Met à jour la position du slider en temps réel
        if (videoPlayer != null && timeSlider != null && videoPlayer.isPlaying)
        {
            timeSlider.value = (float)videoPlayer.time;
        }
    }

    // Méthode appelée quand le slider est déplacé
    public void OnSliderValueChanged()
    {
        if (videoPlayer != null)
        {
            videoPlayer.time = timeSlider.value;
        }
    }

    // Méthode pour démarrer/arrêter la vidéo
    public void TogglePlayPause()
    {
        if (videoPlayer != null)
        {
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Pause();
            }
        }
    }

    // Méthode appelée quand la vidéo se termine
    private void OnVideoEnded(VideoPlayer vp)
    {
        isPlaying = false;
        timeSlider.value = 0f;
        videoPlayer.time = 0f;
    }
}