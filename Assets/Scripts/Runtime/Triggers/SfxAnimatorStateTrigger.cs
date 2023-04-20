using System;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Audio.Runtime.Data;
using Obert.Audio.Runtime.ScriptableObjects;
using UnityEngine;
using UnityEngine.Animations;

namespace Obert.Audio.Runtime.Triggers
{
    public sealed class SfxAnimatorStateTrigger : StateMachineBehaviour, ISfxAnimatorStateTrigger
    {
        [SerializeField, SfxTag] private string tag;
        [SerializeField] private SfxAudioClipBag bag;
        [SerializeField] private AudioClip audioClip;

        private ISfxPlayer[] _players;

        public string Tag => tag;
        public ISfxAudioClipBag Bag => bag;
        public IAudioClip AudioClip => new UnityAudioClip(audioClip);

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            CheckPlayersOrThrow(animator);

            ExecuteTrigger();
        }

        private void ExecuteTrigger()
        {
            foreach (var player in _players)
            {
                player.PlaySfx(this);
            }
        }

        private void CheckPlayersOrThrow(Animator animator)
        {
            if (!animator) return;

            var players = animator.gameObject
                .GetComponents<Component>()
                .OfType<SfxPlayerFacade>()
                .Select(x => x.InternalController)
                .ToArray();

            _players = players;

            if (_players == null)
                throw new NullReferenceException($"No players found on object: {animator.gameObject}");
        }

        private void OnDestroy()
        {
            _players = null;
        }
    }
}