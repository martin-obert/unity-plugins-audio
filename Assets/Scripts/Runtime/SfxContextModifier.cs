using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public abstract class SfxContextModifier<TSfxContext> : MonoBehaviour where TSfxContext : class
    {
        [SerializeField] private ContextualSfxManager sfxManager;

        protected virtual TSfxContext ModifySfxContextOnTriggerEnter(TSfxContext context, Collider currentCollider) =>
            context;

        protected virtual TSfxContext ModifySfxContextOnTriggerExit(TSfxContext context, Collider currentCollider) =>
            context;

        protected virtual TSfxContext ModifySfxContextOnTriggerStay(TSfxContext context, Collider currentCollider) =>
            context;

        protected virtual TSfxContext ModifySfxContextOnCollisionEnter(TSfxContext context, Collision collision) =>
            context;

        protected virtual TSfxContext ModifySfxContextOnCollisionExit(TSfxContext context, Collision collision) =>
            context;

        protected virtual TSfxContext ModifySfxContextOnCollisionStay(TSfxContext context, Collision collision) =>
            context;

        private void OnTriggerEnter(Collider other) => TriggerUpdateContext(ModifySfxContextOnTriggerEnter, other);
        private void OnTriggerExit(Collider other) => TriggerUpdateContext(ModifySfxContextOnTriggerExit, other);
        private void OnTriggerStay(Collider other) => TriggerUpdateContext(ModifySfxContextOnTriggerStay, other);

        private void OnCollisionEnter(Collision collision) => CollisionUpdateContext(ModifySfxContextOnCollisionEnter, collision);
        private void OnCollisionExit(Collision collision) => CollisionUpdateContext(ModifySfxContextOnCollisionExit, collision);
        private void OnCollisionStay(Collision collision) => CollisionUpdateContext(ModifySfxContextOnCollisionStay, collision);

        private void TriggerUpdateContext(Func<TSfxContext, Collider, TSfxContext> logic, Collider value)
        {
            if (!sfxManager) return;

            sfxManager.SfxContext = logic(sfxManager.SfxContext as TSfxContext, value);
        }
        private void CollisionUpdateContext(Func<TSfxContext, Collision, TSfxContext> logic, Collision value)
        {
            if (!sfxManager) return;

            sfxManager.SfxContext = logic(sfxManager.SfxContext as TSfxContext, value);
        }
    }

    public abstract class SfxContextModifier : MonoBehaviour
    {
        protected abstract object ModifySfxContext(object context);

        public void ModifySfxContext(SfxManager manager)
        {
            if (manager is not ISfxContextProvider provider) return;
            var context = ModifySfxContext(provider.SfxContext);
            provider.SfxContext = context;
        }
    }
}