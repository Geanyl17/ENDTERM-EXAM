using UnityEngine;
<<<<<<< Updated upstream
=======
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GameTech;
using UnityEngine.Networking;
>>>>>>> Stashed changes

namespace GameTech
{
<<<<<<< Updated upstream
    [System.Serializable]
    public struct ShapePrefab
    {
        public HitAreaShape shape;
        public GameObject prefab;
    }

    public ShapePrefab[] notePrefabs;
    public float spawnInterval = 1.5f;
    public float spawnRadius = 6f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnRandomNote();
            timer = 0f;
        }
    }

    void SpawnRandomNote()
    {
        int index = Random.Range(0, notePrefabs.Length);
        ShapePrefab selected = notePrefabs[index];

        // Calculate a random direction around the center
        float angle = Random.Range(0f, 360f);
        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 spawnPos = (Vector2)transform.position + direction * spawnRadius;

        // Instantiate and assign noteShape
        GameObject noteGO = Instantiate(selected.prefab, spawnPos, Quaternion.identity);
        Note note = noteGO.GetComponent<Note>();
        if (note != null)
        {
            note.noteShape = selected.shape;
        }
    }
}
=======
    public class NoteSpawner : MonoBehaviour
    {
        public GameObject circleNotePrefab;
        public GameObject squareNotePrefab;
        public GameObject triangleNotePrefab;
        public GameObject rectangleNotePrefab;
        public GameObject shakeNotePrefab;
        public GlowPulseVisualizer visualizer; // Reference to the visualizer

        private AudioSource music;
        private BeatmapNote[] notes;
        private int nextNoteIndex = 0;
        private bool isShakeNoteActive = false;
        private GameObject currentShakeNote;
        private List<GameObject> activeNotes = new List<GameObject>();

        // Spawn radius and angle settings
        public float spawnRadius = 8f;
        public float minAngle = 0f;
        public float maxAngle = 360f;
        public float angleStep = 45f; // Angle between consecutive notes
        public float noteMovementSpeed = 2.0f; // Units per second
        public float noteRotationSpeed = 180f; // Degrees per second
        public float notePulseSpeed = 2f; // Pulses per second
        public float notePulseAmount = 0.1f; // How much to scale up/down

        void Start()
        {
            // Set up audio source
            music = gameObject.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = false; 
            music.volume = 0.05f; //volume

            // Connect audio to visualizer if it exists
            if (visualizer != null)
            {
                visualizer.audioSource = music;
            }

            // Load and play music
            StartCoroutine(LoadMusicAndBeatmap());
        }

        IEnumerator LoadMusicAndBeatmap()
        {
            // Load beatmap first
            string beatmapPath = Path.Combine(Application.streamingAssetsPath, "beatmap.json");
            string json = File.ReadAllText(beatmapPath);
            notes = JsonHelper.FromJson<BeatmapNote>(json);

            // Load music
            string musicPath = Path.Combine(Application.streamingAssetsPath, "Song", "audio.mp3");
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(musicPath, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    music.clip = DownloadHandlerAudioClip.GetContent(www);
                    music.Play();
                }
                else
                {
                    Debug.LogError("Failed to load music: " + www.error);
                }
            }
        }

        private float timer = 0f;
        private float currentAngle = 0f;

        void Update()
        {
            if (notes == null || nextNoteIndex >= notes.Length) return;

            timer += Time.deltaTime;

            if (timer >= notes[nextNoteIndex].time && !isShakeNoteActive)
            {
                SpawnNote(notes[nextNoteIndex]);
                nextNoteIndex++;
            }
        }

        private void SpawnNote(BeatmapNote note)
        {
            GameObject notePrefab = GetNotePrefab(note.type);
            if (notePrefab == null) return;

            // Calculate spawn position based on angle
            float angleRad = note.angle * Mathf.Deg2Rad;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angleRad) * spawnRadius,
                Mathf.Sin(angleRad) * spawnRadius,
                0
            );

            GameObject noteObj = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
            
            // Set up note movement
            NoteMovement movement = noteObj.AddComponent<NoteMovement>();
            movement.targetPosition = Vector3.zero;
            movement.speed = noteMovementSpeed;
            movement.rotationSpeed = noteRotationSpeed;
            movement.pulseSpeed = notePulseSpeed;
            movement.pulseAmount = notePulseAmount;

            // Rotate note to face center
            float angleToCenter = Mathf.Atan2(-spawnPosition.y, -spawnPosition.x) * Mathf.Rad2Deg;
            noteObj.transform.rotation = Quaternion.Euler(0, 0, angleToCenter);

            if (note.type.ToLower() == "shake")
            {
                currentShakeNote = noteObj;
                ShakeNote shakeNote = noteObj.GetComponent<ShakeNote>();
                if (shakeNote != null)
                {
                    // Set the duration from the JSON if specified, otherwise use default
                    if (note.duration > 0)
                    {
                        shakeNote.duration = note.duration;
                    }
                    // Subscribe to the note's completion
                    shakeNote.OnNoteCompleted += HandleShakeNoteCompleted;
                }
            }
        }

        void HandleShakeNoteCompleted()
        {
            isShakeNoteActive = false;
            currentShakeNote = null;
        }

        public void SpawnShakeNote(float time, float duration = 1.0f)
        {
            GameObject note = Instantiate(shakeNotePrefab, transform.position, Quaternion.identity);
            ShakeNote shakeNote = note.GetComponent<ShakeNote>();
            shakeNote.duration = duration;
            shakeNote.OnNoteCompleted += () => activeNotes.Remove(note);
            activeNotes.Add(note);
        }

        private GameObject GetNotePrefab(string type)
        {
            switch (type.ToLower())
            {
                case "circle": return circleNotePrefab;
                case "square": return squareNotePrefab;
                case "triangle": return triangleNotePrefab;
                case "rectangle": return rectangleNotePrefab;
                case "shake": return shakeNotePrefab;
                default: return null;
            }
        }
    }
}
>>>>>>> Stashed changes
