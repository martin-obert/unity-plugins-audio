using System;
using Obert.Audio.Runtime.Data;

namespace Obert.Audio.Runtime.Abstractions
{
    public interface IThemeAudioPlayer : IDisposable
    {
        void ApplyMood(ThemeMood value);
        void Reset();
        void Update(float deltaTime);
    }
}