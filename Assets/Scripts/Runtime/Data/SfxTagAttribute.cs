using System;
using UnityEngine;

namespace Obert.Audio.Runtime.Data
{
    /// <summary>
    /// Helps organize the tags for SFX players and triggers
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SfxTagAttribute : PropertyAttribute
    {
    }
}