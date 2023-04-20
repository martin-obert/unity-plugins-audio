using System;
using Obert.Audio.Runtime.Abstractions;
using UnityEngine;

namespace Obert.Audio.Runtime.Facades
{
    public sealed class UnityAudioSource : IAudioSource
    {
        private readonly AudioSource _audioSource;

        public UnityAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource ? audioSource : throw new ArgumentNullException(nameof(audioSource));
        }

        public bool CanPlay =>
            _audioSource.enabled && _audioSource.gameObject.activeInHierarchy && !_audioSource.isPlaying;

        public AudioClip Clip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }

        public float CurrentTime => _audioSource.time;

        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }

        public bool IsPlaying => _audioSource.isPlaying;

        public void Play(IAudioClip clip, float timePosition = 0, bool? isLooped = null)
        {
            if (clip is not UnityAudioClip unityAudioClip) return;
            Play(unityAudioClip.AudioClip, timePosition, isLooped);
        }

        public void Stop() => _audioSource.Stop();

        public void Play(AudioClip clip, float timePosition, bool? isLooped = null)
        {
            _audioSource.Stop();

            if (isLooped.HasValue)
            {
                _audioSource.clip = clip;
                _audioSource.loop = true;
                _audioSource.Play();
                _audioSource.time = timePosition;
            }
            else
            {
                _audioSource.clip = null;
                _audioSource.loop = false;
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}