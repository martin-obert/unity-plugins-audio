namespace Obert.Audio.Runtime
{
    public interface IAudioSource
    {
        bool CanPlay { get; }
        void Play(IAudioClip clip);
    }
}