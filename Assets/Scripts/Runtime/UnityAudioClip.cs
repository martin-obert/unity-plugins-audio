using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class UnityAudioClip : IAudioClip
    {
        public UnityAudioClip(AudioClip audioClip)
        {
            AudioClip = audioClip ? audioClip : throw new ArgumentNullException(nameof(audioClip));
        }

        public AudioClip AudioClip { get; }
    }
}