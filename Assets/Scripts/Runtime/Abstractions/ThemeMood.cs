using System;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    [Serializable]
    public sealed class ThemeMood
    {
        [SerializeField] private AnimationCurve blendInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private ThemeMoodBlend[] blends;
        [SerializeField] private bool rewindClip;
        [SerializeField] private bool isLooped;

        public AnimationCurve BlendInCurve => blendInCurve;
        public ThemeMoodBlend[] Blends => blends;
        public bool RewindClip => rewindClip;
        public bool IsLooped => isLooped;
    }
}