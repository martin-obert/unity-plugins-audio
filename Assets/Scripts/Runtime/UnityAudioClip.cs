using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class UnityAudioClip : IAudioClip, ISfxTrigger
    {
        public UnityAudioClip(AudioClip audioClip)
        {
            AudioClip = audioClip ? audioClip : throw new ArgumentNullException(nameof(audioClip));
        }

        public AudioClip AudioClip { get; }
        
        public string Tag => AudioClip.name;
    }
}