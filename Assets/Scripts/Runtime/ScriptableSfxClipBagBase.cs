using System;
using System.Linq;
using Obert.Audio.Runtime.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Obert.Audio.Runtime
{
    [CreateAssetMenu(menuName = "Sfx Bag/Sfx Bag", fileName = "Sfx Bag", order = 0)]
    public class ScriptableSfxClipBagBase : ScriptableObject, ISfxBagFilter
    {
        [Header("Description")] [SerializeField]
        private string bagName;

        [SerializeField] private ClipSelectionType selectionType;

        [SerializeField] private ScriptableSfxBagFilter scriptableSfxBagFilter;

        [Header("Audio Clips")] [SerializeField]
        private AudioClip[] clips;


        public string BagName => bagName;

        protected ISfxBagFilter Filter => scriptableSfxBagFilter;


        public virtual bool Match(ISfxBagFilter filter, object audioContext = null)
        {
            return AudioFilterMatch(filter);
        }

        protected virtual bool AudioFilterMatch(ISfxBagFilter filter)
        {
            return filter != null && Equals(Filter ?? this, filter);
        }

        private int _lastIndex;

        public AudioClip GetNextClip()
        {
            if (clips == null || !clips.Any()) return null;

            if (clips.Length == 1) return clips[0];

            switch (selectionType)
            {
                case ClipSelectionType.RoundRobin:
                    _lastIndex = _lastIndex + 1 >= clips.Length ? 0 : _lastIndex + 1;
                    return clips[_lastIndex];
                case ClipSelectionType.Random:
                    _lastIndex = Random.Range(0, clips.Length);
                    return clips[_lastIndex];
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Equals(ISfxBagFilter x, ISfxBagFilter y)
        {
            if (x == null) return false;
            if (y == null) return false;

            if (!string.IsNullOrWhiteSpace(x.BagName) && !string.IsNullOrWhiteSpace(y.BagName))
                return string.Equals(x.BagName, y.BagName);

            return ReferenceEquals(x, y);
        }

        public int GetHashCode(ISfxBagFilter obj)
        {
            return GetHashCode();
        }
    }
}