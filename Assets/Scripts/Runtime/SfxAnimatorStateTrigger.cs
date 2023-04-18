using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

namespace Obert.Audio.Runtime
{
    public sealed class SfxAnimatorStateTrigger : StateMachineBehaviour
    {
        [SerializeField, SfxTag] private string tag;
        [SerializeField] private SfxAudioClipBag bag;

        private class AnimatorStateTrigger : ISfxTrigger
        {
            public AnimatorStateTrigger(string tag)
            {
                Tag = tag;
            }

            public string Tag { get; }
        }
        
        private ISfxPlayer[] _players;

        
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
                var hasTag = !string.IsNullOrWhiteSpace(tag);
                var hasBag = bag != null;
                
                if (!hasBag && !hasTag)
                {
                    player.PlaySfx(null);
                    return;
                }

                if (hasBag)
                {
                    player.PlaySfx(bag);
                }

                if (hasTag)
                {
                    player.PlaySfx(new AnimatorStateTrigger(tag));
                }
            }
        }

        private void CheckPlayersOrThrow(Animator animator)
        {
            if (!animator) return;

            var players = animator.gameObject
                .GetComponents<Component>()
                .OfType<SfxPlayer>()
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