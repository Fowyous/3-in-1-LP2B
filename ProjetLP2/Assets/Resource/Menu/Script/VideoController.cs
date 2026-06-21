using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// Drives a rules-page tutorial video: play/pause toggle and a scrub slider
/// kept in sync with playback (without fighting the user's own dragging).
/// </summary>
public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Slider timeSlider;
    public bool isPlaying = false;

    private void Start()
    {
        if (videoPlayer != null && timeSlider != null)
        {
            timeSlider.maxValue        =  (float)videoPlayer.length;
            videoPlayer.loopPointReached += OnVideoEnded;
        }
    }

    private void Update()
    {
        if (videoPlayer != null && timeSlider != null && videoPlayer.isPlaying)
        {
            timeSlider.SetValueWithoutNotify((float)videoPlayer.time);
        }
    }

    /// <summary>Called when the user drags the slider; seeks the video to match.</summary>
    public void OnSliderValueChanged()
    {
        if (videoPlayer != null)
        {
            videoPlayer.time = timeSlider.value;
        }
    }

    /// <summary>Toggles between playing and pausing the video.</summary>
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

    /// <summary>Resets playback state and the slider once the video reaches its end.</summary>
    private void OnVideoEnded(VideoPlayer vp)
    {
        isPlaying = false;
        timeSlider.SetValueWithoutNotify(0f);
        videoPlayer.time = 0f;
    }
}