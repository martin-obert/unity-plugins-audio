using UnityEngine;

namespace Obert.Audio.Runtime
{
    public class DirectSfxPlayer : SfxManager
    {
        [SerializeField] private SoundEffectSource source;

        public override void Play(ScriptableSfxClipBagBase bagBase)
        {
            source.PlayClip(bagBase.GetNextClip());
        }

        public override void Stop()
        {
            source.StopPlaying();
        }
    }
}