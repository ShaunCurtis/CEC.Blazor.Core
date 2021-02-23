/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using System.Collections;
using System.Collections.Generic;

namespace CEC.Blazor.Core
{
    public class ModalOptions : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        /// List of options
        /// </summary>
        public static readonly string __Width = "Width";
        public static readonly string __ID = "ID";

        /// <summary>
        /// Modal Dialog Title
        /// </summary>
        public string Title { get; set; } = "Modal Dialog";

        /// <summary>
        /// Bool to control the close button in the Header
        /// </summary>
        public bool ShowCloseButton { get; set; }

        /// <summary>
        /// Bool to control display of the header
        /// </summary>
        public bool HideHeader { get; set; }

        /// <summary>
        /// Dictionary of additional Parameters
        /// Read only.  Use the Dictionary Extension Setters and getters provided through CEC.Blazor.Core namespace
        /// </summary>
        public Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();


        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            foreach (var item in Parameters)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        public T Get<T>(string key)
        {
            if (this.Parameters.ContainsKey(key))
            {
                if (this.Parameters[key] is T t) return t;
            }
            return default;
        }

        public bool TryGet<T>(string key, out T value)
        {
            value = default;
            if (this.Parameters.ContainsKey(key))
            {
                if (this.Parameters[key] is T t)
                {
                    value = t;
                    return true;
                }
            }
            return false;
        }

        public bool Set(string key, object value)
        {
            if (this.Parameters.ContainsKey(key))
            {
                this.Parameters[key] = value;
                return false;
            }
            this.Parameters.Add(key, value);
            return true;
        }

    }
}
