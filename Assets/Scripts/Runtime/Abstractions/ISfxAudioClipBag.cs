using Obert.Audio.Runtime.Abstractions;

namespace Obert.Audio.Runtime
{
    public interface ISfxAudioClipBag
    {
        IAudioClip GetAudioClip();
        bool TagEquals(string sfxTag);
        bool HasTag(string value);
    }
}