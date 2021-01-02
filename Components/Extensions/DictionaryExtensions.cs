/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System;
using System.Collections.Generic;
using System.Text;

namespace CEC.Blazor.Core
{
    /// <summary>
    /// Set of extensions for a Property Collection Dictionary<string, object> to make getting and seting easier
    /// </summary>
    public static class DictionaryExtensions
    {
        public static T Get<T>(this Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                if (dict[key] is T t) return t;
            }
            return default;
        }

        public static bool TryGet<T>(this Dictionary<string, object> dict, string key, out T value)
        {
            value = default;
            if (dict.ContainsKey(key))
            {
                if (dict[key] is T t)
                {
                    value = t;
                    return true;
                }
            }
            return false;
        }

        public static bool Set(this Dictionary<string, object> dict, string key, object value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
                return false;
            }
            dict.Add(key, value);
            return true;
        }

    }
}
