using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAmbientAudioClipMetadata
    {
        string Tag { get; }
        AudioClip AudioClip { get; }
    }
}