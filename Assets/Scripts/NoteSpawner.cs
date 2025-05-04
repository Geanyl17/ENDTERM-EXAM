using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using System;
using GameTech;

namespace GameTech
{
    public class NoteSpawner : MonoBehaviour
    {
        [Serializable]
        private class JsonHelper
        {
            public static T[] FromJson<T>(string json)
            {
                Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{\"items\":" + json + "}");
                return wrapper.items;
            }

            [Serializable]
            private class Wrapper<T>
            {
                public T[] items;
            }
        }

        public GameObject circleNotePrefab;
        public GameObject squareNotePrefab;
        public GameObject triangleNotePrefab;
        public GameObject rectangleNotePrefab;
        public GameObject shakeNotePrefab;
        public GlowPulseVisualizer visualizer;

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
        public float angleStep = 45f;
        public float noteMovementSpeed = 2.0f;
        public float noteRotationSpeed = 180f;
        public float notePulseSpeed = 2f;
        public float notePulseAmount = 0.1f;

        void Start()
        {
            // Set up audio source
            music = gameObject.AddComponent<AudioSource>();
            music.playOnAwake = false;
            music.loop = false;
            music.volume = 1f;

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
            string beatmapPath;
            #if UNITY_ANDROID && !UNITY_EDITOR
                beatmapPath = Path.Combine(Application.streamingAssetsPath, "beatmap.json");
                using (UnityWebRequest www = UnityWebRequest.Get(beatmapPath))
                {
                    yield return www.SendWebRequest();
                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        yield break;
                    }
                    string json = www.downloadHandler.text;
                    notes = JsonHelper.FromJson<BeatmapNote>(json);
                }
            #else
                beatmapPath = Path.Combine(Application.streamingAssetsPath, "beatmap.json");
                string json = File.ReadAllText(beatmapPath);
                notes = JsonHelper.FromJson<BeatmapNote>(json);
            #endif

            // Load music
            string musicPath = Path.Combine(Application.streamingAssetsPath, "Song", "audio.mp3");
            #if UNITY_ANDROID && !UNITY_EDITOR
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(musicPath, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();
                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        yield break;
                    }
                    music.clip = DownloadHandlerAudioClip.GetContent(www);
                    music.Play();
                }
            #else
                using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(musicPath, AudioType.MPEG))
                {
                    yield return www.SendWebRequest();
                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        yield break;
                    }
                    music.clip = DownloadHandlerAudioClip.GetContent(www);
                    music.Play();
                }
            #endif
        }

        private float timer = 0f;

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

            if (note.type.ToLower() == "shake")
            {
                // Spawn shake note in center
                GameObject noteObj = Instantiate(notePrefab, Vector3.zero, Quaternion.identity);
                activeNotes.Add(noteObj);
                
                // Set up shake note behavior
                ShakeNote shakeNote = noteObj.GetComponent<ShakeNote>();
                if (shakeNote != null)
                {
                    // Calculate shrink speed based on duration
                    float shrinkSpeed = 1f / note.duration; // This will make it shrink to fit within the duration
                    shakeNote.shrinkSpeed = shrinkSpeed;
                    shakeNote.OnNoteCompleted += HandleShakeNoteCompleted;
                }
                currentShakeNote = noteObj;
                isShakeNoteActive = true;
            }
            else
            {
                // Calculate spawn position based on angle for regular notes
                float angleRad = note.angle * Mathf.Deg2Rad;
                Vector3 spawnPosition = new Vector3(
                    Mathf.Cos(angleRad) * spawnRadius,
                    Mathf.Sin(angleRad) * spawnRadius,
                    0
                );

                GameObject noteObj = Instantiate(notePrefab, spawnPosition, Quaternion.identity);
                activeNotes.Add(noteObj);
            
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
            }
        }

        void HandleShakeNoteCompleted()
        {
            if (currentShakeNote != null)
            {
                activeNotes.Remove(currentShakeNote);
                Destroy(currentShakeNote);
                currentShakeNote = null;
            }
            isShakeNoteActive = false;
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

        public bool IsAllNotesSpawned()
        {
            bool result = notes != null && nextNoteIndex >= notes.Length;
            return result;
        }

        public bool HasActiveNotes()
        {
            bool result = activeNotes.Count > 0 || isShakeNoteActive;
            return result;
        }

        public void RemoveActiveNote(GameObject note)
        {
            activeNotes.Remove(note);
        }
    }
}
