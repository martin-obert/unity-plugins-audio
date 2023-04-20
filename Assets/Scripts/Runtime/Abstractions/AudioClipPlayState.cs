using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public sealed class AudioClipPlayState
    {
        public AudioClipPlayState(AudioClip clip)
        {
            Clip = clip;
        }

        public AudioClip Clip { get; }
        public float LastPosition { get; set; }
    }
}