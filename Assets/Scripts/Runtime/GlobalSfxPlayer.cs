using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    /// <summary>
    /// Allows to play SFX even if the caller is already gone.
    /// Also defines max concurrent SFX be played.
    /// </summary>
    public sealed class GlobalSfxPlayer : MonoBehaviour
    {
        [SerializeField] private SfxPlayer player;

        public sealed class Controller : ISfxPlayer
        {
            private readonly ISfxPlayer _player;

            public Controller(ISfxPlayer player)
            {
                _player = player ?? throw new ArgumentNullException(nameof(player));
            }
            
            public void PlaySfx(ISfxTrigger trigger)
            {
                _player.PlaySfx(trigger);
            }

            public void PlaySfx(AudioClip clip)
            {
                _player.PlaySfx(new UnityAudioClip(clip));
            }
        }

        private void Start()
        {
            if (player != null)
            {
                if (Instance == null)
                {
                    Instance = new Controller(player.InternalController);
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    Destroy(this);
                }
                
            }
        }

        public static Controller Instance { get; set; }
    }
}