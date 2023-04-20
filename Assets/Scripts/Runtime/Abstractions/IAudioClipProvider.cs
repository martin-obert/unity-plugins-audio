namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAudioClipProvider
    {
        IAudioClip ProvideClipContainingTag(string audioTag);
    }
}