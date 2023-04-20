using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAudioSource
    {
        bool CanPlay { get; }
        AudioClip Clip { get; set; }
        float CurrentTime { get; }
        float Volume { get; set; }
        bool IsPlaying { get; }
        void Play(IAudioClip clip, float timePosition = 0, bool? isLooped = null);
        void Stop();
        void Play(AudioClip clip, float timePosition, bool? isLooped = null);
    }
}