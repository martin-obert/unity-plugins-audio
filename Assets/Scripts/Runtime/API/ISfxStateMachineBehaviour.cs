using System;

namespace Obert.Audio.Runtime.API
{
    public interface ISfxStateMachineBehaviour
    {
        event EventHandler<OnStateChanged> PlayAudio;
    }
}