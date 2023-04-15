using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class SfxPlayer : MonoBehaviour
    {
        [SerializeField] private SfxAudioClipBag audioClipBag;
        [SerializeField] private AudioSource[] audioSources;

        [Tooltip("On what triggers does this react")] [SerializeField]
        private SfxTrigger[] triggers;

        private Controller _controller;
        public ISfxPlayer InternalController => _controller;

        private void Awake()
        {
            Assert.IsNotNull(audioClipBag, "audioClipBag != null");
            var sources = audioSources
                .Select(x => (IAudioSource)new UnityAudioSource(x))
                .ToArray();
            _controller = new Controller(sources, audioClipBag, triggers);
        }

        private void Start()
        {
           
        }

        private sealed class Controller : ISfxPlayer
        {
            private readonly IAudioSource[] _audioSources;
            private readonly ISfxAudioClipBag _clipBag;
            private readonly SfxTrigger[] _triggers;

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag clipBag, SfxTrigger[] triggers)
            {
                _triggers = triggers;
                _clipBag = clipBag ?? throw new ArgumentNullException(nameof(clipBag));
                _audioSources = audioSources ?? throw new ArgumentNullException(nameof(audioSources));
            }

            public bool CanConsumeTrigger(SfxTrigger trigger) => _triggers.Any(x => ReferenceEquals(x, trigger));

            public void PlaySfx()
            {
                var clip = _clipBag.GetAudioClip();
                var audioSource = _audioSources.FirstOrDefault(x => x.CanPlay);
                audioSource?.Play(clip);
            }
        }
    }
}