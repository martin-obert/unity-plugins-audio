using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAudioSource
    {
        float TotalLength { get; }
        bool CanPlay { get; }
        AudioClip Clip { get; set; }
        float CurrentTime { get; set; }
        float Volume { get; set; }
        float InitialVolume { get; }
        bool IsPlaying { get; }
        bool IsLooped { get; }
        void PlayOneShot(IAudioClip clip);
        void Stop();
        void PlayOneShot(AudioClip clip);
        void Play(ulong? delay = null);
        void Pause();
    }
}