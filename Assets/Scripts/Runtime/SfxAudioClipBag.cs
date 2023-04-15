using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obert.Audio.Runtime
{
    [CreateAssetMenu(menuName = "Audio/Sfx Audio Clip Bag", fileName = "Sfx Audio Clip Bag", order = 0)]
    public sealed class SfxAudioClipBag : ScriptableObject, ISfxAudioClipBag
    {
        [SerializeField] private AudioClip[] clips;
        [SerializeField] private string[] tags;
        
        private int _lastSelected;
        
        [SerializeField] private AudioClipSelectionMethod selectionMethod;
        
        private IAudioClip[] _audioClips;

        public IAudioClip GetAudioClip()
        {
            if (clips == null || !clips.Any()) throw new Exception($"No audio clips for {name}");

            _audioClips ??= clips
                .Select(x => new UnityAudioClip(x))
                .OfType<IAudioClip>()
                .ToArray();

            switch (selectionMethod)
            {
                case AudioClipSelectionMethod.Random:
                    var random = _audioClips[Random.Range(0, _audioClips.Length)];
                    return random;
                case AudioClipSelectionMethod.RoundRobin:
                    var result = _audioClips[_lastSelected];
                    _lastSelected = _lastSelected + 1 >= _audioClips.Length ? 0 : _lastSelected + 1;
                    return result;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool HasTag(string tag) => tags.Contains(tag);
    }
}