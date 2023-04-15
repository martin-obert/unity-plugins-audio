using UnityEngine;

namespace Obert.Audio.Runtime
{
    [CreateAssetMenu(menuName = "Audio/Sfx Trigger", fileName = "Sfx Trigger", order = 0)]
    public sealed class SfxTrigger : ScriptableObject
    {
        [SerializeField] private string[] tags;
        public string Tag => SfxTagHelpers.GetTag(tags);
    }
}