namespace Obert.Audio.Runtime
{
    public interface ISfxAnimatorStateTrigger : ISfxTrigger
    {
        ISfxAudioClipBag Bag { get; }
        IAudioClip AudioClip { get; }
    }
}