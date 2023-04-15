namespace Obert.Audio.Runtime
{
    public interface ISfxPlayer
    {
        bool CanConsumeTrigger(SfxTrigger trigger);
        void PlaySfx();
    }
}