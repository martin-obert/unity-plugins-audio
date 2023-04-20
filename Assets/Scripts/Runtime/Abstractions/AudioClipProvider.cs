using System;
using System.Linq;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class AudioClipProvider : IAudioClipProvider
    {
        [SerializeField] private UnityAmbientAudioClipMetadata[] clips;

        public IAudioClip ProvideClipContainingTag(string audioTag)
        {
            var clipMetadata = clips.FirstOrDefault(x => x.Tag.Equals(audioTag));
            
            return clipMetadata == null ? null : new UnityAudioClip(clipMetadata.AudioClip);
        }
    }
}