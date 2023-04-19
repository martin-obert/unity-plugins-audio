using System.Linq;
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