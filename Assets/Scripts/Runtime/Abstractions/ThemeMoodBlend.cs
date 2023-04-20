using System;
using Obert.Audio.Runtime.Data;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class ThemeMoodBlend
    {
        [SerializeField] private MoodBlendType blendType;
        [SerializeField] private float amount;
        [SerializeField, SfxTag] private string tag;
        public float Amount => amount;
        public MoodBlendType BlendType => blendType;
        public string Tag => tag;
    }
}