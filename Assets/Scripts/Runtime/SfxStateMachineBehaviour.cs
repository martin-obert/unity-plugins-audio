using System;
using Obert.Audio.Runtime.API;
using UnityEngine;
using UnityEngine.Animations;

namespace Obert.Audio.Runtime
{
    public sealed class SfxStateMachineBehaviour : StateMachineBehaviour, ISfxStateMachineBehaviour
    {
        [Serializable]
        public sealed class StateChangeData
        {
            [SerializeField] private ScriptableSfxBagFilter bagFilter;

            [SerializeField] private ScriptableSfxClipBagBase bagBase;
            public ISfxBagFilter Filter => bagFilter == null ? bagBase : bagFilter;
        }

        public event EventHandler<OnStateChanged> PlayAudio;

        [Space] [Header("Audio SFX")] [SerializeField]
        private StateChangeData onStateEnter;

        [SerializeField] private StateChangeData onStateExit;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) =>
            Play(animator, stateInfo, layerIndex, onStateEnter.Filter);

        private void Play(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, ISfxBagFilter sfxBagFilter)
        {
            if (sfxBagFilter == null) return;
            PlayAudio?.Invoke(this, new OnStateChanged(animator, stateInfo, layerIndex, sfxBagFilter));
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) =>
            Play(animator, stateInfo, layerIndex, onStateExit.Filter);
    }
}