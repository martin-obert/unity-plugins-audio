using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Common.Runtime.Extensions;

namespace Obert.Audio.Runtime
{
    public interface ISfxFilter
    {
        ISfxAudioClipBag[] Filter(ISfxAudioClipBag[] source, string explicitTag = null);
        void AddOptionalTag(string value);
        void AddRequiredTag(string value);
        void RemoveRequiredTag(string value);
        void RemoveOptionalTag(string value);
    }

    internal class SfxFilter : ISfxFilter
    {
        private readonly HashSet<string> _optionalTags = new();
        private readonly HashSet<string> _requiredTags = new();

        public ISfxAudioClipBag[] Filter(ISfxAudioClipBag[] source, string explicitTag = null)
        {
            var tags = Enumerable.Empty<string>();


            if (!string.IsNullOrWhiteSpace(explicitTag))
            {
                tags = SfxTagHelpers.GetTagValues(explicitTag);
            }

            if (_requiredTags.Any())
            {
                tags = tags.Union(_requiredTags);
            }

            var tag = SfxTagHelpers.GetTag(tags);
            var clipBags = source
                .Where(x =>
                    x.HasTag(tag) || _optionalTags.Any(x.HasTag)
                );
            
            return clipBags.ToArray();
        }

        public void AddOptionalTag(string value)
        {
            HashSetAddRange(_optionalTags, value);
        }

        public void AddRequiredTag(string value)
        {
            HashSetAddRange(_requiredTags, value);
        }

        public void RemoveRequiredTag(string value)
        {
            HashSetRemoveRange(_requiredTags, value);
        }

        public void RemoveOptionalTag(string value)
        {
            HashSetRemoveRange(_optionalTags, value);
        }

        private static void HashSetRemoveRange(HashSet<string> source, string value)
        {
            var tags = SfxTagHelpers.GetTagValues(value);
            foreach (var tag in tags)
            {
                source.Remove(tag);
            }
        }

        private static void HashSetAddRange(HashSet<string> source, string value)
        {
            var tags = SfxTagHelpers.GetTagValues(value);
            foreach (var tag in tags)
            {
                source.Add(tag);
            }
        }
    }
}