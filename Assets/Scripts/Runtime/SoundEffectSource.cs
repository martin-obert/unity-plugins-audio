using System;
using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    [Serializable]
    public class SoundEffectSource : ISoundEffectSource
    {
        [SerializeField] private AudioSource audioSource;

        [Tooltip("Per second")] [SerializeField]
        private float maxSfxFrequency;

        private float _lastSfxPlayed;

        public void PlayClip(AudioClip clip)
        {
            if (clip == null) return;

            if (!CanPlay)
            {
                return;
            }

            _lastSfxPlayed = Time.timeSinceLevelLoad;

            audioSource.PlayOneShot(clip);
        }

        public bool CanPlay => audioSource != null && !audioSource.isPlaying && (maxSfxFrequency <= 0 || maxSfxFrequency > 0 &&
            _lastSfxPlayed + maxSfxFrequency > Time.timeSinceLevelLoad);

        public void StopPlaying()
        {
            if(!audioSource) return;
            audioSource.Stop();
        }
    }
}