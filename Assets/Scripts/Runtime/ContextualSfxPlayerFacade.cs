using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class ContextualSfxPlayerFacade : SfxPlayerFacade
    {
        [SerializeField] private SfxAudioClipBag[] audioClipBags;

        private IFilteredSfxPlayer _controller;

        private void Awake()
        {
            Assert.IsNotNull(audioClipBags, "audioClipBag != null");
            var sfxAudioClipBags = audioClipBags.OfType<ISfxAudioClipBag>().ToArray();

            _controller = new Controller(AudioSources, sfxAudioClipBags);
        }

        private sealed class Controller : SfxPlayer, IFilteredSfxPlayer
        {
            private readonly ISfxAudioClipBag[] _clipBags;
            public ISfxFilter Filter { get; } = new SfxFilter();

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag[] clipBags) : base(audioSources)
            {
                _clipBags = clipBags ?? throw new ArgumentNullException(nameof(clipBags));
            }

            protected override IAudioClip[] GetFromTag(string tag)
            {
                if (string.IsNullOrWhiteSpace(tag)) return null;
                var clipBags = Filter.Filter(_clipBags, tag);
                return clipBags.Select(x => x.GetAudioClip()).ToArray();
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