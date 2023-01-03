using System.Collections.Generic;

namespace Obert.Audio.Runtime.API
{
    public interface ISfxBagFilter : IEqualityComparer<ISfxBagFilter>
    {
        string BagName { get; }
    }
}