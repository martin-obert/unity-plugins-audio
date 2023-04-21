using System;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Audio.Runtime.Data;
using UnityEngine;

namespace Obert.Audio.Runtime.Services
{
    [Serializable]
    public sealed class AudioClipProvider : IAudioClipProvider
    {
        [SerializeField] private UnityAmbientAudioClipMetadata[] clips;

        public IAudioSource[] ProvideClipContainingTag(string audioTag)
        {
            var clipMetadata = clips.Where(x => x.Tag.Equals(audioTag));
            
            return clipMetadata.Select(x=>x.AudioSource).ToArray();
        }
    }
}