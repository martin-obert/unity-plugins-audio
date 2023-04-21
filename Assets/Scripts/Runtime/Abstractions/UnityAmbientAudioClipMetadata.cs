using System;
using Obert.Audio.Runtime.Data;
using Obert.Audio.Runtime.Facades;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class UnityAmbientAudioClipMetadata : IAmbientAudioClipMetadata
    {
        [SerializeField, SfxTag] private string tag;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private string resourceId;
        public string Tag => tag;

        private UnityAudioSource _instance;
        public IAudioSource AudioSource => GetAudioSource();

        private IAudioSource GetAudioSource()
        {
            if (_instance != null) return _instance;
            if (!string.IsNullOrWhiteSpace(resourceId))
            {
                return _instance = new UnityAudioSource(audioSource, resourceId);
            }

            return _instance = new UnityAudioSource(audioSource);
        }

    }
}