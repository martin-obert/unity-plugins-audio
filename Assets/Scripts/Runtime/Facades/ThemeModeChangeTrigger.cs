using Obert.Audio.Runtime.Data;
using UnityEngine;

namespace Obert.Audio.Runtime.Facades
{
    public class ThemeModeChangeTrigger : MonoBehaviour
    {
        [SerializeField] private ThemeMood mood;
        public void Trigger()
        {
            ThemeAudioPlayerFacade.Instance.ApplyMood(mood);
        }
    }
}