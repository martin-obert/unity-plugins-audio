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
                var players = _players.Where(x => x.CanConsumeTrigger(trigger)).ToArray();
                foreach (var player in players)
                {
                    player.PlaySfx();
                }
            }
        }

        private void Start()
        {
            _controller = new Controller(players.Select(x => x.InternalController).ToArray());
        }

        public void PlaySfx(Object signal)
        {
            Debug.Log(signal?.GetType());
            if (signal is SfxTrigger trigger)
                _controller.ConsumerSfxTrigger(trigger);
        }
    }
}