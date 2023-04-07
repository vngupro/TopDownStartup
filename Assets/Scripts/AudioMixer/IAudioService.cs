using UnityEngine;

internal interface IAudioService
{
    void PlaySound(AUDIO_CHANNEL channel, AudioClip clip);
}
