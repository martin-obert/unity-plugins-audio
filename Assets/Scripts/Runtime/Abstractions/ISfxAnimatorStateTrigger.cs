namespace Obert.Audio.Runtime.Abstractions
{
    public interface ISfxAnimatorStateTrigger : ISfxTrigger
    {
        ISfxAudioClipBag Bag { get; }
        IAudioClip AudioClip { get; }
    }
}