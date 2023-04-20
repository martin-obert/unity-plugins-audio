namespace Obert.Audio.Runtime.Abstractions
{
    /// <summary>
    /// Advanced player for dynamic SFX playing
    /// </summary>
    public interface IFilteredSfxPlayer : ISfxPlayer
    {
        /// <summary>
        /// Filter that separates audio clips, that are relevant for current play
        /// </summary>
        ISfxFilter Filter { get; }
    }
}