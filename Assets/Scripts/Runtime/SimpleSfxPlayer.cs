using System;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class SimpleSfxPlayer : SfxPlayer
    {

        private ISfxPlayer _internalController;

        private sealed class Controller : ISfxPlayer
        {
            private readonly IAudioSource[] _sources;

            public Controller(IAudioSource[] sources)
            {
                sources.ThrowIfEmptyOrNull();
                _sources = sources;
            }

            public void PlaySfx(ISfxTrigger trigger)
            {
                switch (trigger)
                {
                    case IAudioClip clip:
                        PlayClip(clip);
                        return;
                    case ISfxAudioClipBag bag:
                    {
                        var bagClip = bag.GetAudioClip();
                        if (bagClip == null) return;
                        PlayClip(bagClip);
                        return;
                    }
                    default:
                        throw new NotSupportedException(
                            $"Unable to play from trigger of type: {trigger.GetType()}, {trigger}");
                }
            }

            private void PlayClip(IAudioClip clip)
            {
                if (clip == null) throw new ArgumentNullException(nameof(clip));

                _sources.FirstOrDefault(x => x.CanPlay)?.Play(clip);
            }
        }

        private void Awake()
        {
            Assert.IsTrue(AudioSources.IsNotNullOrEmpty(), "AudioSources.IsNotNullOrEmpty()");

            _internalController = new Controller(AudioSources);
        }

        public override ISfxPlayer InternalController => _internalController;
    }
}