namespace Obert.Audio.Runtime
{
    public interface ISfxPlayer
    {
        void PlaySfx(string tag);
        void PlaySfx(ISfxAudioClipBag bag);
    }
}