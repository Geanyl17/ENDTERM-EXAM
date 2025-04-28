using UnityEngine;
using UnityEngine.Video;
using GameTech;

public class VideoBackground : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip backgroundVideo;
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float darkenAmount = 0.80f;
    
    private NoteSpawner noteSpawner;
    private bool isFadingOut = false;
    private float startAlpha;
    private bool isVideoPrepared = false;
    private SpriteRenderer darkOverlay;

    private void Start()
    {
        // Set up video player
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
            if (videoPlayer == null)
            {
                return;
            }
        }

        // Configure video player
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.aspectRatio = VideoAspectRatio.FitHorizontally;
        videoPlayer.playbackSpeed = 1f;

        // Create dark overlay
      

        if (backgroundVideo != null)
        {
            // Add event handlers
            videoPlayer.prepareCompleted += OnVideoPrepared;
            videoPlayer.errorReceived += OnVideoError;
            videoPlayer.started += OnVideoStarted;

            // Set the video clip and try to play
            videoPlayer.clip = backgroundVideo;
            videoPlayer.source = VideoSource.VideoClip;
            videoPlayer.Play();
            isVideoPrepared = true;
        }

        // Get reference to NoteSpawner
        noteSpawner = FindFirstObjectByType<NoteSpawner>();
    }


    private void OnVideoPrepared(VideoPlayer vp)
    {
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
    }

    private void OnVideoError(VideoPlayer vp, string message)
    {
    }

    private void Update()
    {
        if (!isVideoPrepared)
        {
            return;
        }

        if (!isFadingOut && noteSpawner != null)
        {
            // Check if song is about to end
            if (noteSpawner.IsAllNotesSpawned() && !noteSpawner.HasActiveNotes())
            {
                StartCoroutine(FadeOutVideo());
            }
        }
    }

    private System.Collections.IEnumerator FadeOutVideo()
    {
        isFadingOut = true;
        float timer = 0f;
        float startVolume = videoPlayer.GetDirectAudioVolume(0);
        Color startColor = darkOverlay.color;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeOutDuration;
            
            // Fade audio
            float volume = Mathf.Lerp(startVolume, 0f, t);
            videoPlayer.SetDirectAudioVolume(0, volume);
            
            // Fade overlay
            darkOverlay.color = new Color(0, 0, 0, Mathf.Lerp(startColor.a, 0f, t));
            
            yield return null;
        }

        // Stop video
        videoPlayer.Stop();
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.errorReceived -= OnVideoError;
            videoPlayer.started -= OnVideoStarted;
        }
    }
} 