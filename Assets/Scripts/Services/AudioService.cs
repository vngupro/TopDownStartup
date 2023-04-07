using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

enum AUDIO_CHANNEL
{
    BGM,
    SFX,
    NONE
}

public class AudioService : MonoBehaviour, IAudioService
{
    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _masterMixer = Resources.Load("AudioMixer/MasterMixer") as AudioMixer;
        _audioSource = gameObject.AddComponent<AudioSource>();
    }

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

    void IAudioService.PlaySound(AUDIO_CHANNEL channel, AudioClip clip)
    {
        if (channel == AUDIO_CHANNEL.BGM)
            _audioSource.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("bgm")[0];
        else if (channel == AUDIO_CHANNEL.SFX)
            _audioSource.outputAudioMixerGroup = _masterMixer.FindMatchingGroups("sfx")[0];

        _audioSource.PlayOneShot(clip);
    }
}
