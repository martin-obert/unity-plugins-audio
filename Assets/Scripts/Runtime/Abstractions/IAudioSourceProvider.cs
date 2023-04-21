using System;

namespace Obert.Audio.Runtime.Abstractions
{
    /// <summary>
    /// Provides audio source from the pool. One audio source is associate with single audio clip.
    /// </summary>
    public interface IAudioSourceProvider : IDisposable
    {
        /// <summary>
        /// Provides audio source, that matches tag exactly
        /// </summary>
        /// <param name="audioTag">Tag to be matched by <see cref="string.Equals(string)"/></param>
        /// <returns></returns>
        IAudioSource[] ProvideClipContainingTag(string audioTag);
    }
}