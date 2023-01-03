using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class OnStateChanged
    {
        public OnStateChanged(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            ISfxBagFilter sfxData)
        {
            Animator = animator;
            StateInfo = stateInfo;
            LayerIndex = layerIndex;
            SfxData = sfxData;
        }

        public ISfxBagFilter SfxData { get; }
        public Animator Animator { get; }
        public AnimatorStateInfo StateInfo { get; }
        public int LayerIndex { get; }
    }
}