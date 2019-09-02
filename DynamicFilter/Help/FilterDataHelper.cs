using DynamicFilter.Abstract.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFilter.Help
{
    public static class FilterDataHelper
    {
        internal const char LikeSeparator = '%';
        internal const char LikeScreening = '\\';
        internal static readonly char[] _likeSymbols = { '%', '_', '\\' };
        internal static IFilterData ChangeInnerParent(IFilterData oldParent, IFilterData data)
        {
            while (!data.Parent.Equals(oldParent)) data = data.Parent;

            data.Parent = data.Parent.AsRoot();
            data.IsSelection = false;
            return data.Parent;
        }

        internal static List<IFilterData> CreateChildTree(IFilterData data)
        {
            var current = data ?? throw new ArgumentNullException(nameof(data));
            var childTree = new List<IFilterData>();
            while (current != null)
            {
                childTree.Add(current);
                current = current.Parent;
            }
            return childTree;
        }

        internal static string ToLikeString(string value)
        {
            var sb = new StringBuilder(value.Length + value.Length + 2);
            sb.Append(LikeSeparator);

            foreach (var ch in value) sb.Append(LikeConvert(ch)).Append(LikeSeparator);

            return sb.ToString();
        }

        private static string LikeConvert(char ch) => _likeSymbols.Contains(ch) ? $"{LikeScreening}{ch}" : ch.ToString();

        public static bool IsTrue(string value) => value == "1" || value.Equals("да", StringComparison.OrdinalIgnoreCase) || value.Equals("yes", StringComparison.OrdinalIgnoreCase);
        public static bool IsFalse(string value) => value == "0" || value.Equals("нет", StringComparison.OrdinalIgnoreCase) || value.Equals("no", StringComparison.OrdinalIgnoreCase);

    }
}
