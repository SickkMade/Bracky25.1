using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource aSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        aSource.volume = AudioManager.Instance.SfxVolume;
    }
}
