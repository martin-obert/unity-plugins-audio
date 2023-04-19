using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Audio.Runtime
{
    public sealed class StaticBagSfxPlayerFacade : SfxPlayerFacade
    {
        [SerializeField] private SfxAudioClipBag bag;

        private ISfxPlayer _internalController;

        private class Controller : SfxPlayer
        {
            private readonly ISfxAudioClipBag _clipBag;

            public Controller(IAudioSource[] audioSources, ISfxAudioClipBag clipBag) : base(audioSources)
            {
                _clipBag = clipBag ?? throw new ArgumentNullException(nameof(clipBag));
            }

            protected override IAudioClip GetAudioClipFromBag(ISfxAudioClipBag bag)
            {
                if (bag != _clipBag)
                {
                    Debug.LogWarning("Bag received is not the same as local, processing local");
                }

                return _clipBag.GetAudioClip();
            }
        }

        private void Awake()
        {
            _internalController = new Controller(AudioSources, bag);
        }

        public override ISfxPlayer InternalController => _internalController;
    }
}