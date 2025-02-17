using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource sfxAudio;
    [SerializeField] private AudioSource localAudio;
    private float _musicAudio = 1;
    private float _sfxAudio = 1;
    public float MusicVolume {
        get{ return _musicAudio; }
        set {
            _musicAudio = value;
            musicAudio.volume = value;
        }
    }
    public float SfxVolume {
        get{ return _sfxAudio; }
        set {
            _sfxAudio = value;
            sfxAudio.volume = value;
        }
    }

    public void PlayOneShot(AudioClip audioClip)
    {
        sfxAudio.PlayOneShot(audioClip);
    }

    /// <summary>
    ///  Plays an audioclip at a location. Subject to doppler & spatial blend
    /// </summary>
    /// <param name="audioClip">Audio Clip to Play</param>
    /// <param name="pos">Location to play the sound</param>
    /// <param name="volumeAdj">Volume adjustment. Clamped to 0-1. Default = 1</param>
    public void PlayOneShot(AudioClip audioClip, Vector3 pos, float volumeAdj = 1)
    {
        Debug.Log("Playing Audio at a location");
        AudioSource localSource = Instantiate(localAudio, pos, Quaternion.identity);
        localSource.volume = SfxVolume * Mathf.Clamp(volumeAdj, 0, 1);
        localSource.enabled = true;
        localSource.PlayOneShot(audioClip);
        localSource.pitch = UnityEngine.Random.Range(0.8f, 1.25f);
        float clipLength = audioClip.length;

        Destroy(localSource.gameObject, clipLength);
    }

    // Static instance of GameManager
    public static AudioManager Instance { get; private set; }

    // why is this so well commented it looks ai :sob: I SWEAR I WROTE THIS LOL
    private void Awake()
    {
        // Check if Instance already exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes
        }
        else
        {
            Destroy(gameObject); // Enforce singleton pattern
        }

    }

}