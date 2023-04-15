using System.Linq;

namespace Obert.Audio.Runtime
{
    public static class SfxTagHelpers
    {
        public static string GetTag(string[] fragments) => string.Join(";", fragments.OrderBy(x => x));
    }
}