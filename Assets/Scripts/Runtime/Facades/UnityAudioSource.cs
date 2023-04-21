using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Audio.Runtime.Abstractions;
using Obert.Common.Runtime.Tasks;
using UnityEngine;
using Logger = Obert.Common.Runtime.Logging.Logger;

namespace Obert.Audio.Runtime.Facades
{
    public sealed class UnityAudioSource : IAudioSource
    {
        private readonly AudioSource _audioSource;
        private readonly string _resourceId;
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public UnityAudioSource(AudioSource audioSource)
        {
            _audioSource = audioSource ? audioSource : throw new ArgumentNullException(nameof(audioSource));
            InitialVolume = audioSource.volume;
            Init();
        }

        public UnityAudioSource(AudioSource audioSource, string resourceId)
        {
            _audioSource = audioSource ? audioSource : throw new ArgumentNullException(nameof(audioSource));
            _resourceId = resourceId;
            Init();
        }

        private void Init()
        {
            InitialVolume = _audioSource.volume;
            if (!string.IsNullOrWhiteSpace(_resourceId))
                UniTask.Void(async cancellationToken =>
                {
                    try
                    {
                        await UniTask.SwitchToMainThread(cancellationToken);
                        var result = await Resources
                            .LoadAsync<AudioClip>(_resourceId)
                            .ToUniTask(cancellationToken: cancellationToken);

                        _audioSource.clip = (AudioClip)result;
                        if (IsPlaying)
                            Play();
                    }
                    catch (Exception e)
                    {
                        Logger.Instance.LogException(e);
                    }
                }, _cancellationTokenSource.Token);
        }

        public float TotalLength => _audioSource.clip != null ? _audioSource.clip.length : 0;
        public bool IsLooped => _audioSource.loop;

        public bool CanPlay =>
            _audioSource.enabled && _audioSource.gameObject.activeInHierarchy && !_audioSource.isPlaying;

        public AudioClip Clip
        {
            get => _audioSource.clip;
            set => _audioSource.clip = value;
        }

        public float CurrentTime
        {
            get => _audioSource.time;
            set => _audioSource.time = value;
        }

        public float Volume
        {
            get => _audioSource.volume;
            set => _audioSource.volume = value;
        }

        public float InitialVolume { get; private set; }

        public bool IsPlaying { get; private set; }

        public void PlayOneShot(IAudioClip clip)
        {
            if (clip is not UnityAudioClip unityAudioClip) return;
            PlayOneShot(unityAudioClip.AudioClip);
        }

        public void Stop()
        {
            IsPlaying = false;
            _audioSource.Stop();
        }

        public void Play(ulong? delay = null)
        {
            if (delay.HasValue)
            {
                _audioSource.Play(delay.Value);
            }

            IsPlaying = true;
            _audioSource.Play();
        }

        public void Pause()
        {
            IsPlaying = false;
            _audioSource.Pause();
        }

        public void PlayOneShot(AudioClip clip)
        {
            IsPlaying = true;
            _audioSource.PlayOneShot(clip);
        }

        public void Release()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();
            _cancellationTokenSource?.Dispose();
            Stop();

            if (!string.IsNullOrWhiteSpace(_resourceId) && _audioSource.clip != null)
            {
                Resources.UnloadAsset(_audioSource.clip);
            }
        }
    }
}