using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Audio.Runtime
{
    public sealed class AnimatorEventSfxObserver : MonoBehaviour
    {
        [SerializeField] private SfxPlayer[] players;

        private Controller _controller;

        private class Controller
        {
            private readonly ISfxPlayer[] _players;

            public Controller(ISfxPlayer[] players)
            {
                _players = players ?? throw new ArgumentNullException(nameof(players));
            }

            public void ConsumerSfxTrigger(SfxTrigger trigger)
            {
                foreach (var player in _players)
                {
                    player.PlaySfx(trigger.Tag);
                }
            }

            public void PlayBag(ISfxAudioClipBag bag)
            {
                foreach (var player in _players)
                {
                    player.PlaySfx(bag);
                }
            }
        }

        private void Start()
        {
            _controller = new Controller(players.Select(x => x.InternalController).ToArray());
        }

        public void PlaySfx(Object signal)
        {
            switch (signal)
            {
                case SfxTrigger trigger:
                    _controller.ConsumerSfxTrigger(trigger);
                    break;
                case ISfxAudioClipBag bag:
                    _controller.PlayBag(bag);
                    break;
            }
        }
    }
}