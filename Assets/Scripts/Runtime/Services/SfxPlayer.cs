using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using Obert.Common.Runtime.Extensions;

namespace Obert.Audio.Runtime.Services
{
    public class SfxPlayer : ISfxPlayer
    {
        private readonly IAudioSource[] _sources;

        public SfxPlayer(IAudioSource[] sources)
        {
            sources.ThrowIfEmptyOrNull();
            _sources = sources;
        }

        public void PlaySfx(ISfxTrigger trigger)
        {
            var clips = GetFromTag(trigger?.Tag);
            if (clips is { Length: > 0 })
            {
                PlayClips(clips);
                return;
            }

            var clip = GetClip(trigger);
            if (clip != null)
                PlayClip(clip);
        }

        protected virtual IAudioClip GetClip(ISfxTrigger trigger)
        {
            if (trigger == null) return null;

            return trigger switch
            {
                IAudioClip clip => clip,
                ISfxAudioClipBag bag => GetAudioClipFromBag(bag),
                ISfxAnimatorStateTrigger stateTrigger =>
                    stateTrigger.AudioClip ?? GetAudioClipFromBag(stateTrigger.Bag),
                _ => throw new NotSupportedException(
                    $"Unable to play from trigger of type: {trigger.GetType()}, {trigger}")
            };
        }

        protected virtual IAudioClip[] GetFromTag(string tag)
        {
            return null;
        }

        protected virtual IAudioClip GetAudioClipFromBag(ISfxAudioClipBag bag)
        {
            return bag.GetAudioClip();
        }

        private void PlayClips(IEnumerable<IAudioClip> clips)
        {
            foreach (var clip in clips)
            {
                if (clip == null) continue;
                PlayClip(clip);
            }
        }

        private void PlayClip(IAudioClip clip)
        {
            if (clip == null) throw new ArgumentNullException(nameof(clip));

            _sources.FirstOrDefault(x => x.CanPlay)?.PlayOneShot(clip);
        }
    }
}