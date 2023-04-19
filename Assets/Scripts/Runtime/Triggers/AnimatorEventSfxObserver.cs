using System;
using System.Linq;
using Obert.Audio.Runtime.Abstractions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Obert.Audio.Runtime
{
    public sealed class AnimatorEventSfxObserver : MonoBehaviour
    {
        [SerializeField] private SfxPlayerFacade[] players;

        private Controller _controller;

        private class Controller
        {
            private readonly ISfxPlayer[] _players;

            public Controller(ISfxPlayer[] players)
            {
                _players = players ?? throw new ArgumentNullException(nameof(players));
            }


            public void ConsumeTrigger(ISfxTrigger bag)
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
            if (signal == null)
            {
                _controller.ConsumeTrigger(null);
            }

            switch (signal)
            {
                case ISfxTrigger trigger:
                    _controller.ConsumeTrigger(trigger);
                    break;
            }
        }
    }
}