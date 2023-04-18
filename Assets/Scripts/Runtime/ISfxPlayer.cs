namespace Obert.Audio.Runtime
{
    public interface ISfxPlayer
    {
        void PlaySfx(ISfxTrigger trigger);
    }
    
    public interface IFilteredSfxPlayer : ISfxPlayer
    {
        ISfxFilter Filter { get; }
    }
}