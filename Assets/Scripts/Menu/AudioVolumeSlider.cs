using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))][SelectionBase]
public class AudioVolumeSlider : MonoBehaviour
{
    [SerializeField]
    AudioMixer MainMixer;

    [SerializeField]
    string VolumeString = string.Empty;


    Slider VolumeSlider;

    void Start()
    {
        VolumeSlider = GetComponent<Slider>();

        VolumeSlider.onValueChanged.AddListener((float volume) => SetVolume(volume));
    }

    void SetVolume(float volume)
    {
        if (!string.IsNullOrEmpty(VolumeString))
        {
            if (volume <= -40f)
                volume = -80f;

            MainMixer?.SetFloat(VolumeString, volume);
        }
    }
}
