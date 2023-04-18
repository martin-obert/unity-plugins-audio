using System;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public sealed class AutomaticGunSample : MonoBehaviour
    {
        [SerializeField] private AutomaticGunSfx sfx;

        [SerializeField] private int magSize = 4;
        [SerializeField] private float fireFreq = .4f;

        private int _currentMagSize;

        private float _lastFired;

        private void Awake()
        {
            _currentMagSize = magSize;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (_lastFired + fireFreq > Time.timeSinceLevelLoad) return;
                _lastFired = Time.timeSinceLevelLoad;
                sfx.TriggerHeld(_currentMagSize);
                _currentMagSize = Mathf.Clamp(_currentMagSize - 1, 0, magSize);
            }
            else
            {
                sfx.TriggerReleased();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _currentMagSize = magSize;
            }
        }
    }
}