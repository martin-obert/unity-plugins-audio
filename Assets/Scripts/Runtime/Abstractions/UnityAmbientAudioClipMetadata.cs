using System;
using Obert.Audio.Runtime.Data;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class UnityAmbientAudioClipMetadata : IAmbientAudioClipMetadata
    {
        [SerializeField, SfxTag] private string tag;
        [SerializeField] private AudioClip clip;

        public string Tag => tag;

        public AudioClip AudioClip => clip;
    }
}