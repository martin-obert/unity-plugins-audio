using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorBagNameEventObserver : MonoBehaviour
    {
        [SerializeField] private SfxManager sfxManager;


        public void Sfx(string data)
        {
            if (sfxManager == null)
            {
                Debug.LogWarning("No audio manager was assigned");
                return;
            }
            
            if (string.IsNullOrWhiteSpace(data))
            {
                Debug.LogException(new ArgumentNullException(nameof(data)));
                return;
            }

            sfxManager.Play(new NamedBagFilter(data));
        }
    }
}