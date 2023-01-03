using System.Linq;
using Obert.Audio.Runtime.API;
using UnityEngine;
using UnityEngine.Events;

namespace Obert.Audio.Runtime
{
    public class ContextualSfxManager : SfxManager, ISfxContextProvider
    {
        private ISoundEffectPlayerBase[] _soundEffectContainers;

        private object _sfxContext;

        public object SfxContext
        {
            get => _sfxContext;
            set
            {
                if (value == _sfxContext) return;
                _sfxContext = value;
                onContextChanged?.Invoke();
            }
        }

        [SerializeField] private UnityEvent onContextChanged;

        [SerializeField] private SoundEffectPlayerBase[] containers;

        protected virtual void Awake()
        {
            _soundEffectContainers = containers.OfType<ISoundEffectPlayerBase>().ToArray();
        }

        public override void Play(NamedBagFilter namedBagFilter)
        {
            foreach (var container in _soundEffectContainers)
            {
                container.PlayRandomClip(namedBagFilter, SfxContext);
            }
        }

        public override void Play(ScriptableSfxBagFilter scriptableSfxBagFilter)
        {
            foreach (var container in _soundEffectContainers)
            {
                container.PlayRandomClip(scriptableSfxBagFilter, SfxContext);
            }
        }

        public override void Play(ScriptableSfxClipBagBase bagBase)
        {
            foreach (var container in _soundEffectContainers)
            {
                container.PlayRandomClip(bagBase, SfxContext);
            }
        }

        public override void Stop(NamedBagFilter namedBagFilter)
        {
            StopInternal(namedBagFilter);
        }

        public override void Stop(ScriptableSfxClipBagBase bagBase)
        {
            StopInternal(bagBase);
        }

        public override void Stop(ScriptableSfxBagFilter scriptableSfxBagFilter)
        {
            StopInternal(scriptableSfxBagFilter);
        }

        public override void Stop()
        {
            foreach (var container in _soundEffectContainers)
            {
                container.StopPlaying();
            }
        }

        private void StopInternal(ISfxBagFilter namedBagFilter)
        {
            foreach (var container in _soundEffectContainers.Where(x => x.IsMatch(namedBagFilter)))
            {
                container.StopPlaying();
            }
        }
    }
}