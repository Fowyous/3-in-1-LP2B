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
        if (videoPlayer != null && timeSlider != null && videoPlayer.isPlaying)
        {
            timeSlider.value = (float)videoPlayer.time;
        }
    }
    public void OnSliderValueChanged()
    {
        if (videoPlayer != null)
        {
            videoPlayer.time = timeSlider.value;
        }
    }

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

    private void OnVideoEnded(VideoPlayer vp)
    {
        isPlaying = false;
        timeSlider.value = 0f;
        videoPlayer.time = 0f;
    }
}