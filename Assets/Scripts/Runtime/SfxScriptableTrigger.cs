using UnityEngine;

namespace Obert.Audio.Runtime
{

    public interface ISfxScriptableTrigger : ISfxTrigger
    {
        
    }
    [CreateAssetMenu(menuName = "Audio/Sfx Trigger", fileName = "Sfx Trigger", order = 0)]
    public sealed class SfxScriptableTrigger : ScriptableObject, ISfxScriptableTrigger
    {
        [SerializeField, SfxTag] private string tag;
        public string Tag => tag;
    }
}