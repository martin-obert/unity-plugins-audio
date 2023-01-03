namespace Obert.Audio.Runtime.API
{
    public interface ISoundEffectPlayerBase
    {
        void PlayRandomClip(ISfxBagFilter filter, object audioContext = null);
        bool IsMatch(ISfxBagFilter namedBagFilter);
        void StopPlaying();
    }
}