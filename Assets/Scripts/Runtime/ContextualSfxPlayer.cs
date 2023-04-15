using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class ContextualSfxPlayer : SfxPlayer
    {
        [SerializeField] private SfxAudioClipBag[] audioClipBags;

        private ISfxPlayer _controller;

        private void Awake()
        {
            Assert.IsNotNull(audioClipBags, "audioClipBag != null");
            var sfxAudioClipBags = audioClipBags.OfType<ISfxAudioClipBag>().ToArray();

            _controller = new Controller(AudioSources, sfxAudioClipBags);
        }

        private sealed class Controller : ISfxPlayer
        {
            private readonly IAudioSource[] _audioSources;
            private readonly ISfxAudioClipBag[] _clipBags;

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag[] clipBags)
            {
                _clipBags = clipBags ?? throw new ArgumentNullException(nameof(clipBags));
                _audioSources = audioSources ?? throw new ArgumentNullException(nameof(audioSources));
            }

            public void PlaySfx(string tag)
            {
                var clipBags = _clipBags
                    .Where(x => x.HasTag(tag)).ToArray();

                foreach (var bag in clipBags) PlaySfx(bag);
            }

            public void PlaySfx(ISfxAudioClipBag bag)
            {
                var clip = bag.GetAudioClip();
                var audioSource = _audioSources.FirstOrDefault(x => x.CanPlay);
                audioSource?.Play(clip);
            }
        }

        public override ISfxPlayer InternalController => _controller;
    }
}