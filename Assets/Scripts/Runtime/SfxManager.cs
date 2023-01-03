using System;
using Obert.Audio.Runtime.API;
using UnityEngine;

namespace Obert.Audio.Runtime
{
    public abstract class SfxManager : MonoBehaviour, ISfxManager
    {
        public void Play(ISfxBagFilter filter)
        {
            switch (filter)
            {
                case NamedBagFilter namedBagFilter:
                    Play(namedBagFilter);
                    break;
                case ScriptableSfxBagFilter scriptableSfxBagFilter:
                    Play(scriptableSfxBagFilter);
                    break;
                case ScriptableSfxClipBagBase bagBase:
                    Play(bagBase);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Stop(ISfxBagFilter filter)
        {
            switch (filter)
            {
                case NamedBagFilter namedBagFilter:
                    Stop(namedBagFilter);
                    break;
                case ScriptableSfxBagFilter scriptableSfxBagFilter:
                    Stop(scriptableSfxBagFilter);
                    break;
                case ScriptableSfxClipBagBase bagBase:
                    Stop(bagBase);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public virtual void Play(NamedBagFilter namedBagFilter)
        {
            throw new NotImplementedException();
        }

        public virtual void Play(ScriptableSfxBagFilter scriptableSfxBagFilter)
        {
            throw new NotImplementedException();
        }

        public virtual void Play(ScriptableSfxClipBagBase bagBase)
        {
            throw new NotImplementedException();
        }

        public virtual void Stop(NamedBagFilter namedBagFilter)
        {
            throw new NotImplementedException();
        }

        public virtual void Stop(ScriptableSfxBagFilter scriptableSfxBagFilter)
        {
            throw new NotImplementedException();
        }

        public virtual void Stop(ScriptableSfxClipBagBase bagBase)
        {
            throw new NotImplementedException();
        }
        
        public virtual void Stop()
        {
            throw new NotImplementedException();
        }
    }
}