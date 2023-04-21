using System;
using System.Linq;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
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