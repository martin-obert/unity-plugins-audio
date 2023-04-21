using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Audio.Runtime.Data;
using Obert.Audio.Runtime.Facades;
using Obert.Audio.Runtime.Services;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public class ThemeAudioPlayerFacade : MonoBehaviour
    {
        [SerializeField] private AudioSourceProvider sourceProvider;
        [SerializeField, SfxTag] private string defaultTag;
        [SerializeField] private float defaultValue;

        private static IThemeAudioPlayer _instance;

        public static IThemeAudioPlayer Instance => _instance;
        private Action _onDispose;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = new ThemeAudioPlayer(sourceProvider, defaultTag, defaultValue);
            _onDispose = () =>
            {
                _instance?.Dispose();
                _instance = null;
            };
            DontDestroyOnLoad(gameObject);
        }

        public void ResetMood()
        {
            _instance?.Reset();
        }

        private void Update()
        {
            _instance?.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _onDispose?.Invoke();
        }

        private sealed class ThemeAudioPlayer : IThemeAudioPlayer
        {
            private readonly IAudioSourceProvider _sourceProvider;
            private IAudioSource _mainAudioSource;
            private IAudioSource _blendAudioSource;

            private readonly IDictionary<string, float> _weightedTags = new Dictionary<string, float>();
            private readonly string _defaultTag;
            private readonly float _defaultWeight;
            private float _progress = 0;
            private float _progressMax = 0;
            public bool IsComplete => _animationCurve == null || _progress >= _progressMax;
            private AnimationCurve _animationCurve;

            public void Update(float deltaTime)
            {
                CheckNextClip();
                if (IsComplete)
                {
                    if (_blendAudioSource is { IsPlaying: true })
                    {
                        _blendAudioSource.Pause();
                    }

                    _animationCurve = null;
                    return;
                }

                _progress = Mathf.Clamp(_progress + deltaTime, 0, _progressMax);

                _mainAudioSource.Volume = Mathf.Min(_mainAudioSource.InitialVolume,
                    _mainAudioSource.InitialVolume * _animationCurve.Evaluate(_progress));

                if (_blendAudioSource.IsPlaying)
                    _blendAudioSource.Volume = Mathf.Min(_blendAudioSource.InitialVolume,
                        _blendAudioSource.InitialVolume * _animationCurve.Evaluate(_progressMax - _progress));
            }

            private void CheckNextClip()
            {
                if (!_mainAudioSource.IsPlaying || _mainAudioSource.IsLooped) return;

                if (_mainAudioSource.CurrentTime + .5f < _mainAudioSource.TotalLength)
                    return;

                Debug.Log("Clip is ending and no loop is set, so picking next audio");
                ContinueTheme();
            }

            private void SetBlend(AnimationCurve curve)
            {
                _animationCurve = curve;
                _progress = 0;
                _progressMax = curve.keys.Max(x => x.time);
            }

            public ThemeAudioPlayer(
                IAudioSourceProvider sourceProvider,
                string defaultTag,
                float defaultWeight)
            {
                _sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
                _defaultTag = defaultTag;
                _defaultWeight = defaultWeight;
                Reset();
            }


            public void Reset()
            {
                _weightedTags.Clear();

                if (string.IsNullOrWhiteSpace(_defaultTag)) return;

                _weightedTags.Add(_defaultTag, _defaultWeight);

                PlayClipByTag(_defaultTag, _defaultWeight);
            }

            public void ApplyMood(ThemeMood value)
            {
                foreach (var blend in value.Blends)
                {
                    ApplyBlend(blend);
                }

                var (key, weightAmount) = _weightedTags.OrderByDescending(x => x.Value).FirstOrDefault();
                PlayClipByTag(key, weightAmount, value);
            }

            private void ContinueTheme(ThemeMood themeMood = null)
            {
                var (key, weightAmount) = _weightedTags.OrderByDescending(x => x.Value).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(key)) return;

                Debug.Log($"Selecting audio source with tag: {key}, weight: {weightAmount}");
                var audioSources = _sourceProvider.ProvideClipContainingTag(key);
                if (audioSources.IsNullOrEmpty())
                {
                    Debug.Log($"No audio source found for tag: {key}");
                    return;
                }

                var audioSource = audioSources.FirstOrDefault(x => x != _mainAudioSource && x != _blendAudioSource);
                if (audioSource == null)
                {
                    Debug.Log("All audio sources for this theme are already playing, returning");
                    return;
                }
                
                if (audioSource == _mainAudioSource && _mainAudioSource != null)
                {
                    Debug.Log($"Main AudioSource is already playing same clip: {_mainAudioSource.Clip}");
                    return;
                }

                if (_mainAudioSource != null)
                    Debug.Log($"Selected audio source with clip: {_mainAudioSource.Clip}");

                _blendAudioSource?.Pause();

                if (themeMood != null)
                {
                    _blendAudioSource = _mainAudioSource;
                }

                _mainAudioSource = audioSource;
                _mainAudioSource.Play();
                if (themeMood != null)
                    SetBlend(themeMood.BlendInCurve);
            }

            private void PlayClipByTag(string key, float weightAmount, ThemeMood themeMood = null)
            {
                if (string.IsNullOrWhiteSpace(key)) return;

                Debug.Log($"Selecting audio source with tag: {key}, weight: {weightAmount}");
                var audioSources = _sourceProvider.ProvideClipContainingTag(key);
                if (audioSources.IsNullOrEmpty())
                {
                    Debug.Log($"No audio source found for tag: {key}");
                    return;
                }

                if (audioSources.Any(x => x == _mainAudioSource))
                {
                    Debug.Log($"Theme didn't changed continuing with current theme: {key}");
                    return;
                }

                var audioSource = audioSources.FirstOrDefault(x => x != _mainAudioSource && x != _blendAudioSource);
                if (audioSource == null)
                {
                    Debug.Log("All audio sources for this theme are already playing, returning");
                    return;
                }

                if (audioSource == _mainAudioSource)
                {
                    Debug.Log($"Main AudioSource is already playing same clip: {audioSource.Clip}");
                    return;
                }

                Debug.Log($"Selected audio source with clip: {audioSource.Clip}");

                _blendAudioSource?.Pause();

                if (themeMood != null)
                {
                    _blendAudioSource = _mainAudioSource;
                }

                _mainAudioSource = audioSource;
                _mainAudioSource.Play();
                if (themeMood != null)
                    SetBlend(themeMood.BlendInCurve);
            }

            private void ApplyBlend(ThemeMoodBlend blend)
            {
                var tags = SfxTagHelpers.GetTagValues(blend.Tag);
                foreach (var tag in tags)
                {
                    if (!_weightedTags.TryGetValue(tag, out var weight))
                    {
                        _weightedTags.Add(tag, ProcessBlend(0, blend.Amount, blend.BlendType));
                    }
                    else
                    {
                        _weightedTags[tag] = ProcessBlend(weight, blend.Amount, blend.BlendType);
                    }
                }
            }

            private static float ProcessBlend(float currentWeight, float amount, MoodBlendType blendType)
            {
                switch (blendType)
                {
                    case MoodBlendType.Additive:
                        return currentWeight + amount;
                    case MoodBlendType.Multiply:
                        return currentWeight * amount;
                    case MoodBlendType.Override:
                        return amount;
                    case MoodBlendType.Clear:
                        return 0;
                    case MoodBlendType.Unknown:
                    default:
                        throw new ArgumentOutOfRangeException(nameof(blendType), blendType, null);
                }
            }

            public void Dispose()
            {
                _weightedTags.Clear();
                _sourceProvider?.Dispose();
            }
        }
    }
}