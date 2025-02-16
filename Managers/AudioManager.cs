using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource sfxAudio;
    private float _musicAudio;
    private float _sfxAudio;
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

    public void PlayOneShot(AudioClip audioClip){
        sfxAudio.PlayOneShot(audioClip);
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