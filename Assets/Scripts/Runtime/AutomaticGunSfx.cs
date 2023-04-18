using System;
using System.Linq;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class AutomaticGunSfx : MonoBehaviour
    {
        [SerializeField] private bool repeatDrySound;
        [SerializeField] private AudioSource[] sources;

        [SerializeField] private AudioClip startSound;
        [SerializeField] private AudioClip shotWithTail;
        [SerializeField] private AudioClip tailSound;
        [SerializeField] private SfxAudioClipBag drySound;
        [SerializeField] private SfxAudioClipBag shotBag;
        [SerializeField] private SfxAudioClipBag magUnloadBag;
        [SerializeField] private SfxAudioClipBag magLoadBag;
        [SerializeField] private SfxAudioClipBag cockBag;
        [SerializeField] private SfxAudioClipBag safetySwitch;

        private bool _isTriggerHeld;
        private bool _drySoundPlayed;
        private bool _tailPlayed;

        public void UnloadMag() => Play(magUnloadBag.GetAudioClip());
        public void LoadMag() => Play(magLoadBag.GetAudioClip());
        public void CockRifle() => Play(cockBag.GetAudioClip());
        public void SafetySwitch() => Play(safetySwitch.GetAudioClip());
        
        private void Play(AudioClip clip)
        {
            var source = sources.FirstOrDefault(x => !x.isPlaying);
            if (source == null)
            {
                Debug.LogError($"No source to play on: {clip}");
                return;
            }

            source.PlayOneShot(clip);
        }

        public void TriggerHeld(int magSize)
        {
            Debug.Log($"Mag size: {magSize}");
            if (magSize == 1)
            {
                if (!_isTriggerHeld)
                {
                    Debug.Log("Start Sound");
                    Play(startSound);
                }

                _isTriggerHeld = true;

                if (!_tailPlayed)
                {
                    _tailPlayed = true;
                    Debug.Log("Shot With Tail Sound");
                    Play(shotWithTail);
                }

                return;
            }

            if (magSize <= 0)
            {
                _isTriggerHeld = true;

                if (_drySoundPlayed && !repeatDrySound) return;

                _drySoundPlayed = true;
                Debug.Log("Dry Sound");
                Play(drySound.GetAudioClip());
                return;
            }

            if (!_isTriggerHeld)
            {
                _isTriggerHeld = true;
                Debug.Log("Start Sound");
                Play(startSound);
            }

            _tailPlayed = false;
            _drySoundPlayed = false;
            Debug.Log("Shot Sound");
            Play(shotBag.GetAudioClip());
        }

        private void Play(IAudioClip clip)
        {
            if (clip is not UnityAudioClip audioClip) return;

            Play(audioClip.AudioClip
                ? audioClip.AudioClip
                : throw new ArgumentNullException(nameof(audioClip.AudioClip)));
        }

        public void TriggerReleased()
        {
            if (!_tailPlayed && _isTriggerHeld)
            {
                _tailPlayed = true;
                Debug.Log("Tail Only Sound");
                Play(tailSound);
            }
            _isTriggerHeld = false;
        }
    }
}