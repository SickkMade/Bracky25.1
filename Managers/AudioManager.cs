using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource musicAudio2;
    [SerializeField] private AudioSource sfxAudio;
    [SerializeField] private AudioSource localAudio;

    [SerializeField] private AudioClip mainGameMusic;
    [SerializeField] private AudioClip tunnelsMusic;
    [SerializeField] private AudioClip EnemyDrums;
    [SerializeField] private AudioClip minigameMusic;

    private MusicState musicState = MusicState.Main;
    private bool curChasing = false;
    private bool changeToChase = false;
    [SerializeField, Range(0f, 10f)] float chaseFadeTime = 5f; 
   float curChaseFadeTime = 0f; 

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

    public void Update()
    {
        if (changeToChase && !curChasing)
        {
            float t = musicAudio.time;
            musicAudio2.Play();
            musicAudio2.time = t;
            curChasing = true;
        }

        if (musicState == MusicState.Main)
        {
            if (PlayerManager.Instance.playerData.IsInTunnels)
            {
                float t = musicAudio.time;
                musicAudio.clip = tunnelsMusic; 
                musicAudio.Play();
                musicAudio.time = t;
                musicState = MusicState.Tunnels;
            }
            else
            {
                if(PlayerManager.Instance.playerData.position.y < 30)
                {
                    musicAudio.volume = MusicVolume * (PlayerManager.Instance.playerData.position.y - 25.0f) / 5.0f;
                }
                else
                {
                    musicAudio.volume = MusicVolume;
                }
            }
        }

        if (musicState == MusicState.Tunnels)
        {
            if (!PlayerManager.Instance.playerData.IsInTunnels)
            {
                float t = musicAudio.time;
                musicAudio.clip = mainGameMusic; 
                musicAudio.Play();
                musicAudio.time = t;
                musicState = MusicState.Main;
            }
            else
            {
                if (PlayerManager.Instance.playerData.position.y > 23)
                {
                    musicAudio.volume = MusicVolume * (25.0f - PlayerManager.Instance.playerData.position.y) / 2.0f;
                }
                else
                {
                    musicAudio.volume = MusicVolume;
                }
            }
        }

        if(curChasing)
        {
            if (changeToChase)
            {
                changeToChase = false;
                curChaseFadeTime = chaseFadeTime;
                musicAudio2.volume = MusicVolume;
            }
            else
            {
                curChaseFadeTime -= Time.deltaTime;
                musicAudio2.volume = MusicVolume * (curChaseFadeTime / chaseFadeTime);
                if(curChaseFadeTime < 0)
                {
                    curChasing = false;
                    musicAudio2.Stop();
                }
            }
        }

    }

    public void ChangeToChase()
    {
        Debug.Log("CHASE");
        changeToChase = true;
    }

    public enum MusicState
    {
        Main,
        Tunnels
    }
}