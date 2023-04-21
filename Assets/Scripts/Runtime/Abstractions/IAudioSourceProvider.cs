using System;

namespace Obert.Audio.Runtime.Abstractions
{
    public interface IAudioSourceProvider : IDisposable
    {
        IAudioSource[] ProvideClipContainingTag(string audioTag);
    }
}