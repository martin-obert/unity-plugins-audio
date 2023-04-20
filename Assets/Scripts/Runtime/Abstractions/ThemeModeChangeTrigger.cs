using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
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