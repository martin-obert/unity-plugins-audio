using System;
using System.Linq;
using Obert.Audio.Runtime.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Audio.Runtime
{
    [RequireComponent(typeof(Animator))]
    public sealed class AnimatorObjectEventObserver : MonoBehaviour
    {
        [SerializeField] private SfxManager[] managers;


        public void Sfx(Object data)
        {
            if (managers == null || !managers.Any())
            {
                Debug.LogWarning("No audio manager was assigned");
                return;
            }
            
            if (data is null)
            {
                Debug.LogException(new ArgumentNullException(nameof(data)));
                return;
            }

            if (data is ISfxBagFilter filter)
            {
                foreach (var sfxManager in managers)
                {
                    if(!sfxManager) continue;

                    sfxManager.Play(filter);
                }
                return;
            }

            Debug.LogException(
                new FormatException($"Cannot process animation event value of type {data.GetType()}"));
        }
    }
}