using Obert.Audio.Runtime;
using UnityEngine;

namespace Sample.Basic.Scripts
{
    public class GroundTypeSfxContextModifier : SfxContextModifier<GroundTypeContext>
    {
        protected override GroundTypeContext ModifySfxContextOnTriggerEnter(GroundTypeContext context,
            Collider currentCollider)
        {
            context ??= new GroundTypeContext();
            
            var ground = currentCollider.GetComponent<Ground>();
            
            if (ground)
                context.Type = ground.Type;

            return context;
        }
    }
}