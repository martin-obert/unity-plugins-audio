using Obert.Audio.Runtime;
using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Sample.Basic.Scripts
{
    [CreateAssetMenu(menuName = "Sfx Bag/Samples/Sfx Bag - Ground Specific", fileName = "Sfx Bag - Ground", order = 0)]
    public class GroundTypeScriptableSfxClipBag : ScriptableSfxClipBag<GroundTypeContext>
    {
        [SerializeField] private GroundType groundTypeTag;

        protected override bool Match(ISfxBagFilter filter, GroundTypeContext audioContext = default)
        {
            // Quick check for scriptable object reference
            if (!ReferenceEquals(filter, Filter) || audioContext == null) return false;

            return groundTypeTag == audioContext.Type;
        }
    }
}