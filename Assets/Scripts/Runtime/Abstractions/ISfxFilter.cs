namespace Obert.Audio.Runtime.Abstractions
{
    /// <summary>
    /// Filters out bags that doesn't fit for current SFX play
    /// </summary>
    public interface ISfxFilter
    {
        ISfxAudioClipBag[] Filter(ISfxAudioClipBag[] source, string explicitTag = null);
        void AddOptionalTag(string value);
        void AddRequiredTag(string value);
        void RemoveRequiredTag(string value);
        void RemoveOptionalTag(string value);
    }
}