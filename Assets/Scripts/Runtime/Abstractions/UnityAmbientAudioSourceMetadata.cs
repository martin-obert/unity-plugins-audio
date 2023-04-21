using System;
using Obert.Audio.Runtime.Data;
using Obert.Audio.Runtime.Facades;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class UnityAmbientAudioSourceMetadata : IAmbientAudioClipMetadata, IDisposable
    {
        [SerializeField, SfxTag] private string tag;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private string resourceId;

        public string ResourceId => resourceId;
        public string Tag => tag;

        private UnityAudioSource _unityAudioSource;

        public IAudioSource AudioSource => GetAudioSource();

        private IAudioSource GetAudioSource()
        {
            if (_unityAudioSource != null) return _unityAudioSource;

            if (string.IsNullOrWhiteSpace(resourceId)) return _unityAudioSource = new UnityAudioSource(audioSource);
            return _unityAudioSource = new UnityAudioSource(audioSource, resourceId);
        }

        public void Dispose()
        {
            _unityAudioSource?.Release();
        }
    }
}