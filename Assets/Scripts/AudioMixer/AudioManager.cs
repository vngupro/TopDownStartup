using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _masterMixer;

    public void SetMasterVolume(Slider volume)
    {
        _masterMixer.SetFloat("master", volume.value);
    }

    public void SetBGMVolume(Slider volume)
    {
        _masterMixer.SetFloat("bgm", volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        _masterMixer.SetFloat("sfx", volume.value);
    }
}
