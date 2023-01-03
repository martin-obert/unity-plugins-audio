using UnityEngine;

namespace Obert.Audio.Runtime
{
    public static class Extensions
    {
        public static SfxManager GetSfxManager(this Component component) => !component ? null : component.GetComponent<SfxManager>();
        public static SfxManager GetSfxManager(this GameObject gameObject) => !gameObject ? null : gameObject.GetComponent<SfxManager>();
    }
}