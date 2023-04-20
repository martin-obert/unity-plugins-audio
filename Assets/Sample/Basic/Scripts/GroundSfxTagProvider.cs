using Obert.Audio.Runtime;
using Obert.Audio.Runtime.Data;
using UnityEngine;

namespace Sample.Basic.Scripts
{
    public sealed class GroundSfxTagProvider : MonoBehaviour
    {
        [SerializeField, SfxTag] private string sfxTag;

        public string SfxTag => sfxTag;
    }
}