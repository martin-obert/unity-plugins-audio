using System;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Audio.Runtime.Services;
using Obert.Common.Runtime.Extensions;
using UnityEngine.Assertions;

namespace Obert.Audio.Runtime
{
    public sealed class SimpleSfxPlayerFacade : SfxPlayerFacade
    {
        private ISfxPlayer _internalController;

        private void Awake()
        {
            Assert.IsTrue(AudioSources.IsNotNullOrEmpty(), "AudioSources.IsNotNullOrEmpty()");

            _internalController = new SfxPlayer(AudioSources);
        }

        public override ISfxPlayer InternalController => _internalController;
    }
}