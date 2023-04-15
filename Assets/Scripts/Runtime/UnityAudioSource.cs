using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class UnityAudioSource : IAudioSource
    {
        private readonly AudioSource _audioSource;

        public UnityAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource ? audioSource : throw new ArgumentNullException(nameof(audioSource));
        }

        public bool CanPlay => !_audioSource.isPlaying;

        public void Play(IAudioClip clip)
        {
            if (clip is not UnityAudioClip unityAudioClip) return;

            _audioSource.PlayOneShot(unityAudioClip.AudioClip);
        }
    }
}