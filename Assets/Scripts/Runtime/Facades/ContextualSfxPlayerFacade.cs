using System;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Audio.Runtime.ScriptableObjects;
using Obert.Audio.Runtime.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime.Facades
{
    /// <summary>
    /// Extends <see cref="SimpleSfxPlayerFacade"/> and <see cref="StaticBagSfxPlayerFacade"/> for playing SFX from tag.
    /// Contains methods for tag filtering manipulation <see cref="ISfxFilter"/>
    /// </summary>
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

        /// <inheritdoc cref="ISfxFilter.AddRequiredTag"/>
        public void AddRequiredTag(string value)
        {
            _controller.Filter.AddRequiredTag(value);
        }

        /// <inheritdoc cref="ISfxFilter.RemoveRequiredTag"/>
        public void RemoveRequiredTag(string value)
        {
            _controller.Filter.RemoveRequiredTag(value);
        }

        /// <inheritdoc cref="ISfxFilter.AddOptionalTag"/>
        public void AddOptionalTag(string value)
        {
            _controller.Filter.AddOptionalTag(value);
        }

        /// <inheritdoc cref="ISfxFilter.RemoveOptionalTag"/>
        public void RemoveOptionalTag(string value)
        {
            _controller.Filter.RemoveOptionalTag(value);
        }
    }
}