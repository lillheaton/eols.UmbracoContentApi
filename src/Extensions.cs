using System;
using System.Collections.Generic;
using System.Linq;

namespace EOls.UmbracoContentApi
{
    public static class Extensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            KeyValuePair<TKey, TValue>? result = 
                source
                    .Where(predicate)
                    .Select(x => x as KeyValuePair<TKey, TValue>?)
                    .FirstOrDefault();
            
            if (!result.HasValue)
                return default(TValue);

            return result.Value.Value;
        }
    }
}
