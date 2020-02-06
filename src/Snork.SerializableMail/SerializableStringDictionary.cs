using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Snork.SerializableMail
{
    [Serializable]
    public class SerializableStringDictionary : Dictionary<string, string>
    {
        public SerializableStringDictionary()
        {
        }

        public SerializableStringDictionary(IDictionary<string, string> input) : base(input)
        {
        }

        internal void CopyKeyValuePairs(NameValueCollection destination)
        {
            foreach (var key in Keys)
                destination[key] = this[key];
        }

        internal void CopyKeyValuePairs(StringDictionary destination)
        {
            foreach (var key in Keys)
                destination[key] = this[key];
        }

        public static implicit operator SerializableStringDictionary(NameValueCollection collection)
        {
            var result = new SerializableStringDictionary();
            foreach (string key in collection.Keys)
                result[key] = collection[key];
            return result;
        }

        public static implicit operator SerializableStringDictionary(StringDictionary collection)
        {
            var result = new SerializableStringDictionary();
            foreach (var key in collection.Keys.OfType<string>()) result[key] = collection[key];
            return result;
        }
    }
}