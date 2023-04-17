using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class ContextualSfxPlayer : SfxPlayer
    {
        [SerializeField] private SfxAudioClipBag[] audioClipBags;

        private IFilteredSfxPlayer _controller;

        private void Awake()
        {
            Assert.IsNotNull(audioClipBags, "audioClipBag != null");
            var sfxAudioClipBags = audioClipBags.OfType<ISfxAudioClipBag>().ToArray();

            _controller = new Controller(AudioSources, sfxAudioClipBags);
        }

        private sealed class Controller : IFilteredSfxPlayer
        {
            private readonly IAudioSource[] _audioSources;
            private readonly ISfxAudioClipBag[] _clipBags;
            public ISfxFilter Filter { get; } = new SfxFilter();

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag[] clipBags)
            {
                _clipBags = clipBags ?? throw new ArgumentNullException(nameof(clipBags));
                _audioSources = audioSources ?? throw new ArgumentNullException(nameof(audioSources));
            }

            private void PlaySfxFromTrigger(string tag)
            {
                if (string.IsNullOrWhiteSpace(tag))
                {
                    return;
                }


                var clipBags = Filter.Filter(_clipBags, tag);

                foreach (var bag in clipBags)
                {
                    PlaySfxFromBag(bag);
                }
            }

            public void PlaySfx(ISfxTrigger trigger)
            {
                switch (trigger)
                {
                    case null:
                        return;
                    case ISfxAudioClipBag bag:
                    {
                        if (_clipBags == null || !_clipBags.Any())
                        {
                            PlaySfxFromBag(bag);
                        }

                        return;
                    }
                    default:
                        PlaySfxFromTrigger(trigger.Tag);
                        return;
                }
            }


            private void PlaySfxFromBag(ISfxAudioClipBag audioClipBag)
            {
                var clip = audioClipBag.GetAudioClip();
                var audioSource = _audioSources.FirstOrDefault(x => x.CanPlay);
                audioSource?.Play(clip);
            }
        }

        public override ISfxPlayer InternalController => _controller;

        public void AddRequiredTag(string value)
        {
            _controller.Filter.AddRequiredTag(value);
        }

        public void RemoveRequiredTag(string value)
        {
            _controller.Filter.RemoveRequiredTag(value);
        }

        public void AddOptionalTag(string value)
        {
            _controller.Filter.AddOptionalTag(value);
        }

        public void RemoveOptionalTag(string value)
        {
            _controller.Filter.RemoveOptionalTag(value);
        }
    }
}