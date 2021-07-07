using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("Volume"))
            volumeSlider.value = 0.75f;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        AudioManager.Instance.SetVolume(PlayerPrefs.GetFloat("Volume"));
    }

    public void SetVolume()
    {
        AudioManager.Instance.SetVolume(volumeSlider.value);
    }
}
