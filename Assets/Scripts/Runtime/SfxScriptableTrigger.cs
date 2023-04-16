using UnityEngine;

namespace Obert.Audio.Runtime
{
    [CreateAssetMenu(menuName = "Audio/Sfx Trigger", fileName = "Sfx Trigger", order = 0)]
    public sealed class SfxScriptableTrigger : ScriptableObject, ISfxTrigger
    {
        [SerializeField, SfxTag] private string tag;
        public string Tag => tag;
    }
}