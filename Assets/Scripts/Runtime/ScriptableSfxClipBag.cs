using Obert.Audio.Runtime.API;

namespace Obert.Audio.Runtime
{
    public abstract class ScriptableSfxClipBag<TAudioContext> : ScriptableSfxClipBagBase
    {

        public override bool Match(ISfxBagFilter filter, object audioContext = null)
        {
            if (audioContext is TAudioContext context)
            {
                return base.Match(filter, audioContext) && Match(filter, context);
            }

            return base.Match(filter, audioContext);
        }

        protected abstract bool Match(ISfxBagFilter filter, TAudioContext audioContext = default);
    }
}