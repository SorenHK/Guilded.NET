using System.Collections.Generic;
using System.Linq;

namespace System
{
#if NETSTANDARD2_0

    public class HashCode
    {
        public static int Combine(object obj1, object obj2)
        {
            return Combine(obj1?.GetHashCode(), obj2?.GetHashCode());
        }

        public static int Combine(ICollection<int?> vals)
        {
            return Combine(vals?.Select(v => v ?? 0).ToArray());
        }

        public static int Combine(params int?[] vals)
        {
            return Combine(vals?.Select(v => v ?? 0).ToArray());
        }

        public static int Combine(params int[] vals)
        {
            // Seed and factor from https://stackoverflow.com/a/34006336/723798

            return CombineHashCodesWithFixedSeed(1009, 9176, vals);
        }

        private static int CombineHashCodesWithFixedSeed(int seed, int factor, ICollection<int> vals)
        {
            var hash = seed;

            if (vals != null && vals.Count > 0)
                foreach (var i in vals)
                    hash = (hash * factor) + i;

            return hash;
        }
    }

#endif
}