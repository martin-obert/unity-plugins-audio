using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    /// <summary>
    /// Just wrapper for exposing sfx manager globally.
    /// This should be handy when playing spamming sounds (like particle effects), but don't wont to clog the audio system.
    /// </summary>
    public class GlobalSfxPlayer : MonoBehaviour
    {
        [SerializeField]
        private SfxManager sfxManager;
        
        public static GlobalSfxPlayer Instance { get; private set; }

        public ISfxManager SfxManager => sfxManager;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            if(Instance != this) return;

            Instance = null;
        }
    }
}