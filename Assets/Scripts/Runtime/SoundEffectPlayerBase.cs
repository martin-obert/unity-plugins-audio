using System;
using System.Linq;
using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    [Serializable]
    public class SoundEffectPlayerBase : ISoundEffectPlayerBase
    {
        [SerializeField] private ScriptableSfxClipBagBase[] bags;

        [SerializeField] private SoundEffectSource[] soundEffectSources;
        
        private ISoundEffectSource SoundEffectSource => soundEffectSources.FirstOrDefault(x => x.CanPlay);

        public void PlayRandomClip(ISfxBagFilter filter = null, object audioContext = null)
        {
            var sfxClipBag = filter == default ? bags.First() : bags.FirstOrDefault(x => x.Match(filter, audioContext));
            
            PlayRandomClip(sfxClipBag);
        }

        public bool IsMatch(ISfxBagFilter namedBagFilter) => bags.Any(x => x.Match(namedBagFilter));
        public void StopPlaying()
        {
            foreach (var soundEffectSource in soundEffectSources)
            {
                soundEffectSource.StopPlaying();
            }
        }

        public void PlayRandomClip(ScriptableSfxClipBagBase bagBase)
        {
            if (bagBase == null) return;
            SoundEffectSource?.PlayClip(bagBase.GetNextClip());
        }

    }
}