using System;
using System.Linq;
using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorStateObserver : MonoBehaviour, IDisposable
    {
        private ISfxStateMachineBehaviour[] _triggers;

        [SerializeField] private SfxManager[] managers;

        private void Awake()
        {
            _triggers = GetComponent<Animator>()
                .GetBehaviours<SfxStateMachineBehaviour>()
                .OfType<ISfxStateMachineBehaviour>()
                .ToArray();
        }

        private void Start()
        {
            foreach (var trigger in _triggers)
            {
                trigger.PlayAudio += TriggerOnPlayAudio;
            }
        }

        private void TriggerOnPlayAudio(object sender, OnStateChanged e)
        {
            foreach (var sfxManager in managers)
            {
                if (!sfxManager) continue;
                sfxManager.Play(e.SfxData);
            }
        }


        private void OnDestroy()
        {
            Dispose();
        }


        public void Dispose()
        {
            if (_triggers == null) return;
            foreach (var trigger in _triggers)
            {
                trigger.PlayAudio -= TriggerOnPlayAudio;
            }
        }
    }
}