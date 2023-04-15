using System;
using System.Linq;

namespace Obert.Audio.Runtime
{
    public sealed class DirectSfxPlayer : SfxPlayer
    {
        private ISfxPlayer _internalController;

        private class Controller : ISfxPlayer
        {
            private readonly IAudioSource[] _audioSources;

            public Controller(IAudioSource[] audioSources)
            {
                _audioSources = audioSources ?? throw new ArgumentNullException(nameof(audioSources));
            }

            public void PlaySfx(string tag)
            {
            }

            public void PlaySfx(ISfxAudioClipBag bag)
            {
                var clip = bag.GetAudioClip();
                var source = _audioSources.FirstOrDefault(x => x.CanPlay);
                source?.Play(clip);
            }
        }


        private void Awake()
        {
            _internalController = new Controller(AudioSources);
        }

        public override ISfxPlayer InternalController => _internalController;
    }
}