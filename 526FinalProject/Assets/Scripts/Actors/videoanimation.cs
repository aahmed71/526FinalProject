using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class videoanimation : MonoBehaviour
{
    // Start is called before the first frame update
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        
        if (videoPlayer != null)
        {
            // Subscribe to the loopPointReached event
            videoPlayer.loopPointReached += OnVideoLoopPointReached;

            // Start playing the video
            videoPlayer.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnVideoLoopPointReached(VideoPlayer vp)
    {
        // Set the time to the beginning to make the video loop
        vp.time = 0f;
    }

    // Remember to unsubscribe from the event when the script is disabled or destroyed
    void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoLoopPointReached;
        }
    }
}
