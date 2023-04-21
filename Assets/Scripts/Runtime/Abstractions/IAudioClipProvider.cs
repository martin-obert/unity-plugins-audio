namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAudioClipProvider
    {
        IAudioSource[] ProvideClipContainingTag(string audioTag);
    }
}