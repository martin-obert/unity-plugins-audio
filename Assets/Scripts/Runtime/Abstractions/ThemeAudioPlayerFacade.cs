using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Obert.Audio.Runtime.Data;
using Obert.Audio.Runtime.Facades;
using Obert.Audio.Runtime.Services;
using UnityEngine;

namespace Obert.Audio.Runtime.Abstractions
{
    public class ThemeAudioPlayerFacade : MonoBehaviour
    {
        [SerializeField] private AudioClipProvider clipProvider;
        [SerializeField, SfxTag] private string defaultTag;
        [SerializeField] private float defaultValue;
        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private AudioSource blendAudioSource;

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

            _instance = new ThemeAudioPlayer(clipProvider, new UnityAudioSource(mainAudioSource),
                new UnityAudioSource(blendAudioSource), defaultTag, defaultValue);
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
            private readonly IAudioClipProvider _clipProvider;

            private readonly IAudioSource _mainAudioSource;
            private readonly IAudioSource _blendAudioSource;

            private readonly IDictionary<string, float> _weightedTags = new Dictionary<string, float>();
            private readonly string _defaultTag;
            private readonly float _defaultWeight;
            private readonly IList<AudioClipPlayState> _playStates = new Collection<AudioClipPlayState>();
            private float progress = 0;
            public bool IsComplete => _animationCurve == null || progress >= 1;
            private AnimationCurve _animationCurve;

            public void Update(float deltaTime)
            {
                if (IsComplete)
                {
                    _animationCurve = null;
                    return;
                }

                _mainAudioSource.Volume = _animationCurve.Evaluate(progress);

                if (_blendAudioSource.IsPlaying)
                    _blendAudioSource.Volume = _animationCurve.Evaluate(1 - progress);
                progress = Mathf.Clamp01(progress + deltaTime);
                if (IsComplete)
                {
                    SaveCurrentClipState(_blendAudioSource);
                    _blendAudioSource.Stop();
                    _blendAudioSource.Clip = null;
                }
            }

            private void SetBlend(AnimationCurve curve)
            {
                _animationCurve = curve;
                progress = 0;
            }

            public ThemeAudioPlayer(
                IAudioClipProvider clipProvider,
                IAudioSource mainAudioSource,
                IAudioSource blendAudioSource,
                string defaultTag,
                float defaultWeight)
            {
                _mainAudioSource = mainAudioSource ?? throw new ArgumentNullException(nameof(mainAudioSource));
                _blendAudioSource = blendAudioSource ?? throw new ArgumentNullException(nameof(blendAudioSource));
                _clipProvider = clipProvider ?? throw new ArgumentNullException(nameof(clipProvider));
                _defaultTag = defaultTag;
                _defaultWeight = defaultWeight;
                Reset();
            }


            public void Reset()
            {
                _weightedTags.Clear();
                if (!string.IsNullOrWhiteSpace(_defaultTag))
                {
                    PlayClipByTag(_defaultTag, _defaultWeight);
                }
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

            private void PlayClipByTag(string key, float weightAmount, ThemeMood themeMood = null)
            {
                if (string.IsNullOrWhiteSpace(key)) return;

                Debug.Log($"Selecting clip with tag: {key}, weight: {weightAmount}");

                if (_clipProvider.ProvideClipContainingTag(key) is not UnityAudioClip clip)
                {
                    Debug.Log("No clip found");
                    return;
                }

                var audioClip = clip.AudioClip;

                Debug.Log($"Selected clip: {audioClip}");

                if (audioClip == _mainAudioSource.Clip)
                {
                    Debug.Log($"Main AudioSource is playing same clip: {audioClip}");
                    return;
                }

                var clipState = GetClipState(audioClip);

                if (_mainAudioSource.IsPlaying)
                {
                    _blendAudioSource.Play(_mainAudioSource.Clip, _mainAudioSource.CurrentTime, true);
                }

                _mainAudioSource.Play(clip, timePosition: clipState.LastPosition, true);

                if (themeMood != null)
                    SetBlend(themeMood.BlendInCurve);
            }

            private AudioClipPlayState GetClipState(AudioClip audioClip)
            {
                var clipState = _playStates.FirstOrDefault(x => x.Clip == audioClip);
                if (clipState == null)
                {
                    _playStates.Add(clipState = new AudioClipPlayState(audioClip));
                }

                return clipState;
            }

            private void SaveCurrentClipState(IAudioSource audioSource)
            {
                if (!audioSource.Clip) return;
                var state = _playStates.FirstOrDefault(x => x.Clip == audioSource.Clip);
                if (state == null) return;
                state.LastPosition = audioSource.CurrentTime;
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

            private float ProcessBlend(float currentWeight, float amount, MoodBlendType blendType)
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
                _playStates.Clear();
            }
        }
    }
}