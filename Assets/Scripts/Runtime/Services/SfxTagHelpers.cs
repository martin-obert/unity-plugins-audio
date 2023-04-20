using System;
using System.Collections.Generic;
using System.Linq;
using Obert.Common.Runtime.Extensions;

namespace Obert.Audio.Runtime.Services
{
    public static class SfxTagHelpers
    {
        public static string GetTag(IEnumerable<string> fragments) => string.Join(";", OrderTags(fragments));

        public static string[] GetTagValues(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                ? Array.Empty<string>()
                : value.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] AppendTag(string[] tags, string tag)
        {
            if (HasTag(tags, tag)) return tags;
            return OrderTags(tags.Append(tag)).ToArray();
        }

        public static IOrderedEnumerable<string> OrderTags(this IEnumerable<string> tags)
        {
            return tags.OrderBy(x => x);
        }

        public static bool HasTag(string[] tags, string tag)
        {
            if (tags.IsNullOrEmpty()) return false;
            if (string.IsNullOrWhiteSpace(tag)) return false;

            return tags.Contains(tag);
        }

        public static string[] RemoveTag(IEnumerable<string> tags, string tag)
        {
            return OrderTags(tags.Where(x => x != tag)).ToArray();
        }
    }
}