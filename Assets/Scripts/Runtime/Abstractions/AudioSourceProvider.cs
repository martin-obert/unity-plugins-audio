﻿using System;
using System.Linq;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class AudioSourceProvider : IAudioSourceProvider
    {
        [SerializeField] private UnityAmbientAudioSourceMetadata[] clips;

        public IAudioSource[] ProvideClipContainingTag(string audioTag)
        {
            var clipMetadata = clips.Where(x => x.Tag.Equals(audioTag));

            return clipMetadata.Select(x => x.AudioSource).ToArray();
        }

        public void Dispose()
        {
            foreach (var clip in clips)
            {
                clip.Dispose();
            }
        }
    }
}