using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Audio.Runtime.Facades;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public abstract class SfxPlayerFacade : MonoBehaviour
    {
        [SerializeField] private AudioSource[] audioSources;

        protected IAudioSource[] AudioSources =>
            audioSources.Select(x => new UnityAudioSource(x)).OfType<IAudioSource>().ToArray();

        public abstract ISfxPlayer InternalController { get;  }
    }
}