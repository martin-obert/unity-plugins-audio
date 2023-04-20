using System;
using UnityEngine;

namespace Obert.Audio.Runtime.ScriptableObjects
{
    public sealed class SfxTagProvider : ScriptableObject, ISfxTagProvider
    {
        [SerializeField] private string[] availableTags;

        private static SfxTagProvider _instance;
        public const string Path = "SfxTagProvider";
        public static ISfxTagProvider Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<SfxTagProvider>(Path);
                }

                return _instance;
            }
            set
            {
                if (_instance) throw new Exception("Already set");
                _instance = (SfxTagProvider)value;
            }
        }

        public string[] AvailableTags => availableTags;

        private void OnDestroy()
        {
            Debug.LogWarning("Destroyed");
            if (ReferenceEquals(Instance, this))
            {
                _instance = null;
            }
                
        }
    }
}