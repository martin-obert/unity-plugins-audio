namespace Obert.Audio.Runtime.Data
{
    /// <summary>
    /// Defines a selection method for picking audio clip from for ex.: <see cref="ISfxAudioClipBag"/>
    /// </summary>
    public enum AudioClipSelectionMethod
    {
        Random = 0,
        RoundRobin = 1
    }
}