using System.Collections.Generic;
using System.Linq;

namespace System
{
#if NETSTANDARD2_0

    public static class DictionaryUtils
    {
        public static S GetValueOrDefault<T, S>(this IDictionary<T, S> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];

            return default(S);
        }
    }

#endif
}