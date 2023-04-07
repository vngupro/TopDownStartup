using UnityEngine;
using UnityEngine.UI;

internal interface IAudioService
{
    void PlaySound(AUDIO_CHANNEL channel, AudioClip clip);
    public void SetMasterVolume(Slider volume);
    public void SetBGMVolume(Slider volume);
    public void SetSFXVolume(Slider volume);
}
