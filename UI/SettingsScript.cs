using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        musicSlider.onValueChanged.AddListener (delegate {ChangeMusicSlider();});
        sfxSlider.onValueChanged.AddListener (delegate {ChangeSFXSlider();});
    }

    private void ChangeMusicSlider(){
        AudioManager.Instance.MusicVolume = musicSlider.value;
    }

    private void ChangeSFXSlider(){
        AudioManager.Instance.SfxVolume = sfxSlider.value;
    }
}
