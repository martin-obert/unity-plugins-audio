using System;
using Obert.Audio.Runtime.API;

namespace Obert.Audio.Runtime
{
    public sealed class NamedBagFilter : ISfxBagFilter
    {
        public NamedBagFilter(string bagName = null)
        {
            BagName = bagName;
        }

        public bool Equals(ISfxBagFilter x, ISfxBagFilter y)
        {
            if (x == null || string.IsNullOrWhiteSpace(x.BagName)) return false;
            if (y == null || string.IsNullOrWhiteSpace(y.BagName)) return false;

            return string.Equals(x.BagName, y.BagName);
        }

        public int GetHashCode(ISfxBagFilter obj)
        {
            return GetHashCode();
        }

        public string BagName { get; }
    }
}