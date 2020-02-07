﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.Operations.Reports.Reports.Extensions
{
    public static class IDictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null || key == null)
            {
                return default(TValue);
            }

            dictionary.TryGetValue(key, out var value);

            return value;
        }
    }
}
