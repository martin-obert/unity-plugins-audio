using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public class SingleContainerSfxManager : SfxManager
    {
        [SerializeField] private SoundEffectPlayerBase player;

        public override void Play(NamedBagFilter namedBagFilter) => PlayInternal(namedBagFilter);

        public override void Play(ScriptableSfxBagFilter scriptableSfxBagFilter) =>
            PlayInternal(scriptableSfxBagFilter);

        public override void Play(ScriptableSfxClipBagBase bagBase) => PlayInternal(bagBase);

        private void PlayInternal(ISfxBagFilter filter) => player.PlayRandomClip(filter);

        public override void Stop(NamedBagFilter namedBagFilter) => StopInternal(namedBagFilter);

        public override void Stop(ScriptableSfxBagFilter scriptableSfxBagFilter) =>
            StopInternal(scriptableSfxBagFilter);

        public override void Stop(ScriptableSfxClipBagBase bagBase) => StopInternal(bagBase);

        public override void Stop() => player.StopPlaying();

        private void StopInternal(ISfxBagFilter filter)
        {
            if (!player.IsMatch(filter)) return;
            player.StopPlaying();
        }
    }
}