using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using GameTech;
using UnityEngine.UI;

public class GameEndManager : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 1f;
    [SerializeField] private float delayBeforeSceneChange = 1f;
    [SerializeField] private string gameOverSceneName = "GameOver";
    [SerializeField] private Image fadePanel; // Reference to a black UI panel
    [SerializeField] private GameObject fadeObject; // GameObject with SpriteRenderer to fade
    
    private AudioSource music;
    private NoteSpawner noteSpawner;
    private bool isGameEnding = false;
    private bool isMusicLoaded = false;
    private SpriteRenderer fadeObjectRenderer;

    private void Start()
    {
        noteSpawner = FindFirstObjectByType<NoteSpawner>();
        if (noteSpawner != null)
        {
            StartCoroutine(WaitForMusicLoad());
        }

        // Initialize fade panel
        if (fadePanel != null)
        {
            Color color = fadePanel.color;
            color.a = 0f;
            fadePanel.color = color;
            fadePanel.gameObject.SetActive(true);
        }

        // Initialize fade object
        if (fadeObject != null)
        {
            fadeObjectRenderer = fadeObject.GetComponent<SpriteRenderer>();
            if (fadeObjectRenderer != null)
            {
                Color spriteColor = fadeObjectRenderer.color;
                spriteColor.a = 150f/255f; // Set initial alpha to 150/255
                fadeObjectRenderer.color = spriteColor;
            }
        }
    }

    private IEnumerator WaitForMusicLoad()
    {
        while (noteSpawner.GetComponent<AudioSource>() == null || !noteSpawner.GetComponent<AudioSource>().isPlaying)
        {
            yield return null;
        }

        music = noteSpawner.GetComponent<AudioSource>();
        isMusicLoaded = true;
    }

    private void Update()
    {
        if (!isGameEnding && noteSpawner != null && isMusicLoaded)
        {
            if (noteSpawner.IsAllNotesSpawned() && !noteSpawner.HasActiveNotes())
            {
                StartCoroutine(EndGameSequence());
            }
        }
    }

    private IEnumerator EndGameSequence()
    {
        isGameEnding = true;

        if (music != null && music.isPlaying)
        {
            float startVolume = music.volume;
            float timer = 0f;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime * 2f;
                float t = Mathf.Clamp01(timer / fadeOutDuration);
                
                // Fade music
                music.volume = Mathf.Lerp(startVolume, 0f, t * t);
                
                // Fade screen
                if (fadePanel != null)
                {
                    Color color = fadePanel.color;
                    color.a = t;
                    fadePanel.color = color;
                }

                // Fade object
                if (fadeObjectRenderer != null)
                {
                    Color spriteColor = fadeObjectRenderer.color;
                    spriteColor.a = Mathf.Lerp(150f/255f, 0f, t * t);
                    fadeObjectRenderer.color = spriteColor;
                }
                
                yield return null;
            }

            music.volume = 0f;
            if (fadePanel != null)
            {
                Color color = fadePanel.color;
                color.a = 1f;
                fadePanel.color = color;
            }
            if (fadeObjectRenderer != null)
            {
                Color spriteColor = fadeObjectRenderer.color;
                spriteColor.a = 0f;
                fadeObjectRenderer.color = spriteColor;
            }
        }

        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(gameOverSceneName);
    }
} 