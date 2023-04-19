using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Audio.Runtime
{
    public sealed class StaticBagSfxPlayer : SfxPlayer
    {
        [SerializeField] private SfxAudioClipBag bag;

        private ISfxPlayer _internalController;

        private class Controller : ISfxPlayer
        {
            private readonly IAudioSource[] _audioSources;
            [CanBeNull] private readonly ISfxAudioClipBag _clipBag;

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag clipBag)
            {
                _clipBag = clipBag;
                _audioSources = audioSources ?? throw new ArgumentNullException(nameof(audioSources));
            }

            public void PlaySfx(ISfxTrigger trigger)
            {
                if (trigger is ISfxAudioClipBag bag)
                {
                    if (_clipBag == null)
                    {
                        PlaySfx(bag);
                        return;
                    }

                    if (bag != _clipBag)
                    {
                        Debug.LogWarning(
                            $"{nameof(StaticBagSfxPlayer)} was triggered by explicit bag {(trigger is Object o ? o.ToString() : trigger.ToString())}. Trigger is being ignored");
                        return;
                    }
                }

                if (trigger is UnityAudioClip clip)
                {
                    PlayClip(clip);
                    return;
                }
                
                PlaySfx(_clipBag);
            }

            private void PlaySfx(ISfxAudioClipBag bag)
            {
                var clip = bag.GetAudioClip();
                PlayClip(clip);
            }

            private void PlayClip(IAudioClip clip)
            {
                var source = _audioSources.FirstOrDefault(x => x.CanPlay);
                source?.Play(clip);
            }
        }

        private void Awake()
        {
            _internalController = new Controller(AudioSources, bag);
        }

        public override ISfxPlayer InternalController => _internalController;
    }
}