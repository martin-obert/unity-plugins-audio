namespace Obert.Audio.Runtime
{
    public interface ISfxAudioClipBag
    {
        IAudioClip GetAudioClip();
        bool HasTag(string tag);
    }
}