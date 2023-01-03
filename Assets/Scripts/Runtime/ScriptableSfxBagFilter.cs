using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    [CreateAssetMenu(menuName = "Sfx Bag/Sfx Bag Filter", fileName = "Sfx Bag Filter", order = 0)]
    public class ScriptableSfxBagFilter : ScriptableObject, ISfxBagFilter
    {
        [SerializeField] private string bagName;
        
        public string BagName => bagName;

        public bool Equals(ISfxBagFilter x, ISfxBagFilter y)
        {
            if (x == null) return false;
            if (y == null) return false;

            if (string.IsNullOrWhiteSpace(x.BagName)) return false;
            if (string.IsNullOrWhiteSpace(y.BagName)) return false;

            return x.BagName.Equals(y.BagName);
        }

        public int GetHashCode(ISfxBagFilter obj)
        {
            return GetHashCode();
        }

    }
}